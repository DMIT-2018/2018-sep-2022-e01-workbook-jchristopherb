using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional 
using ChinookSystem.ViewModels;
using WebApp.Helpers;
using ChinookSystem.BLL;
#endregion


namespace WebApp.Pages.SamplePages
{

    public class PlaylistManagementModel : PageModel
    {
        #region Private variables and DI constructor
        // these two local variables hold the link to the specific BLL class.
        private readonly TrackServices _trackServices;
        private readonly PlaylistTrackServices _playlisttrackServices;

        // This code constructor is injecting dependencies. These classes are declared on the Extension Method
        public PlaylistManagementModel(TrackServices trackservices,
                                PlaylistTrackServices playlisttrackservices)
        {
            _trackServices = trackservices;
            _playlisttrackServices = playlisttrackservices;
        }
        #endregion

        #region Messaging and Error Handling
        [TempData]
        public string FeedBackMessage { get; set; }
        
        public string ErrorMessage { get; set; }

        //a get property that returns the result of the lamda action
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBackMessage);

        //used to display any collection of errors on web page
        //whether the errors are generated locally OR come from the class library service methods
        public List<string> ErrorDetails { get; set; } = new();

        //PageModel local error list for collection 
        public List<Exception> Errors { get; set; } = new();

        #endregion

        #region Paginator
        private const int PAGE_SIZE = 5;
        public Paginator Pager { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? currentpage { get; set; }   
        #endregion

        [BindProperty(SupportsGet = true)]
        public string searchBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string searchArg { get; set; }

        [BindProperty(SupportsGet = true)]
        public string playlistname { get; set; }

        public List<TrackSelection> trackInfo { get; set; } 

        public List<PlaylistTrackInfo> qplaylistInfo { get; set; } // Query Model - one way

        // this property will be tied to the INPUT fields of the web page
        // this list is tied to the table data elements for the playlist
        [BindProperty]
        public List<PlaylistTrackTRX> cplaylistInfo { get; set; } // Command Model - since it's two way

        // this property is tied to the form input element located on each of the rows of the track table
        // it will hold the trackid one wishes to attempt to add to the playlist
        [BindProperty]
        public int addtrackid { get; set; }

        public const string USERNAME = "HansenB";
        public void OnGet()
        {
            // this METHOD is executed everytime the page is called for the first time
            // OR
            // whenever a GET request is made to the page SUCH AS RedirectToPage()
            GetTrackInfo();
            GetPlaylist();
        }

        public void GetTrackInfo()
        {
            if (!string.IsNullOrWhiteSpace(searchArg) &&
                            !string.IsNullOrWhiteSpace(searchBy))
            {
                int totalcount = 0;
                int pagenumber = currentpage.HasValue ? currentpage.Value : 1;
                PageState current = new(pagenumber, PAGE_SIZE);
                trackInfo = _trackServices.Track_FetchTracksBy(searchArg.Trim(),
                    searchBy.Trim(), pagenumber, PAGE_SIZE, out totalcount);
                Pager = new(totalcount, current);
            }
        }

        public void GetPlaylist()
        {
            if (!string.IsNullOrWhiteSpace(playlistname))
            {
                string username = USERNAME;
                qplaylistInfo = _playlisttrackServices.PlaylistTrack_FetchPlaylist(playlistname.Trim(), username);
            }
        }
        // IActionResult should have at least Page() and/or RedirectToPage()
        // no changes to the page ie showing error, use Page(), and it will not go to OnGet() method
        // RedirectToPage() - the only displayed are what gathered on GET; inside try{}
        public IActionResult OnPostTrackSearch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchBy))
                {
                    Errors.Add(new Exception("Track search type not selected"));
                }
                if (string.IsNullOrWhiteSpace(searchArg))
                {
                    Errors.Add(new Exception("Track search string not entered"));
                }
                if (Errors.Any())
                {
                    throw new AggregateException(Errors);
                }
                // RedirectToPage() will cause a GET request to be issue (OnGet())
                return RedirectToPage(new
                {
                    searchBy = searchBy.Trim(),
                    searchArg = searchArg.Trim(),
                    playlistname = string.IsNullOrWhiteSpace(playlistname) ? " " : playlistname.Trim()
                });
            }
            catch (AggregateException ex)
            {
                // Displaying all errors on a list. **referencing line 35 on the PlaylistManagement.cshtml**
                ErrorMessage = "Unable to process search";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);
                    
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();
            }
        }

        public IActionResult OnPostFetch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("Enter a playlist name to fetch.");
                }
                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname.Trim()
                });
            }
            catch(Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();

            }
        }

        public IActionResult OnPostAddTrack()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist select first. Enter a playlist name and Fetch");
                }
               
                // Add the code to add a track via the service.
                // the data needed for your call has ALREADY been placed in your local properties
                //  by the use of [BindProperty] which is two-way (output/input)
                // once security is installed, you would be able to obtain the username from the operating system
                string username = USERNAME;
                // call your (transaction) service sending in the expected data
                _playlisttrackServices.PlaylistTrack_AddTrack(playlistname, username, addtrackid);
                FeedBackMessage = "adding the track";
                return RedirectToPage(new
                {
                    searchby = searchBy,
                    searcharg = searchArg,
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {
                              
                ErrorMessage = "Unable to process add track";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                // Since the OnGet() will NOT be called if there is a transaction err,
                //  the catch MUST do the actions of the OnGet
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            
        }

        public IActionResult OnPostRemove()
        {
            try
            {
               //Add the code to process the list of tracks via the service.
               if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist selected first. Enter a playlist name and press Fetch.");
                }
                int oneselection = cplaylistInfo
                                    .Where(x => x.SelectedTrack)
                                    .Count();
                if (oneselection == 0)
                {
                    throw new Exception("You first need to select at least one track before pressing remove.");
                }

                string username = USERNAME;
                //send data to the service
                _playlisttrackServices.PlaylistTrack_RemoveTracks(playlistname, username, cplaylistInfo);

                //success
                FeedBackMessage = "Tracks habe been removed.";

                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process remove tracks";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }

        }

        private Exception GetInnerException(Exception ex)
        {
            while(ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
