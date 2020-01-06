using Contract.Model;
using ETL.Callers;
using ETL.Helpers;
using ETL.Webscraper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static ETL.Webscraper.FilmWebScraper;

namespace ETL.View
{
    public partial class Main : Window
    {
        List<Movie> dbMovies = null;
        List<Movie> moviesToLoad = null;
        Movie movieToEdit = null;
        readonly int proc = Environment.ProcessorCount;
        FilmWebScraper scraper = new FilmWebScraper();
        Task[] tasksArray = null;
        Thread[] threadsArray = null;

        public Main()
        {
            InitializeComponent();

            CheckPassword();

            // USTALAMY LICZBE WATKOW I TASKOW (PROGRAMOWANIE ASYNCHRONICZNE I WIELOWATKOWE)
            threadsArray = new Thread[2];

            // LICZNBA TASKOW JEST ROWNA LICZBIE RDZENI LOGICZNYCH PROCESORA
            tasksArray = new Task[proc];

            // USTAWALY PUNKTY WYJSCIA DO EVENTU ZE SKRAPERA
            scraper.TextBoxValueChanged += OtherWindowOnTextBoxValueChanged;

            WriteToConsole("Application Started");
            WriteToConsole($"Number Of Logical Processors: {Environment.ProcessorCount}");

            ConsoleScrollViewer.ScrollToBottom();
            Search_Async();
        }

        public void CheckPassword()
        {
            InputText enterPassword = new InputText();
            enterPassword.ShowDialog();
            var password = enterPassword.Get();

            DataCallers.Instance.ConnectionString = $"server = 89.68.162.35; port = 4000; userid = mario; password = {password}; database = ETLProject; sslmode = None";

            if (string.IsNullOrEmpty(password) || !DataCallers.Instance.IsServerConnected())
            {
                MessageBox.Show("Incorrect password\nApplication will be closed");
                System.Environment.Exit(1);
            }

            enterPassword = null;
        }

        private void OtherWindowOnTextBoxValueChanged(object sender, TextBoxValueEventArgs e)
        {
            WriteToConsole(e.NewValue);
        }

        private Thread loadingThread;
        public void Search_Async(string txt = null, bool getNewData = true)
        {
            AbortLoading();
            Working(true);
            loadingThread = new Thread(() => Search(txt, getNewData));
            loadingThread.Start();
        }

        public void AbortLoading()
        {
            if (loadingThread != null)
            {
                loadingThread.Abort();
                loadingThread = null;
            }
        }

        private object loadingLock = new object();
        public void Search(string txt = null, bool getNewData = true)
        {
            WriteToConsole("Search initialized");

            // UZYLO OBJECT LOCK, ZABRANIA ON DOSTEP DO OKRESLONEJ CZESCI KODU PRZEZ WIECEJ NIZ JEDEN WATEK
            lock (loadingLock)
            {
                try
                {
                    if (getNewData) dbMovies = UpdateData();

                    Dispatcher.Invoke((Action)(() =>
                    {
                        UpdateView(txt);
                        Working(false);
                    }));
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
        }

        private List<Movie> UpdateData()
        {
            WriteToConsole("UpdateData started");
            var result = DataCallers.Instance.GetAllMovies();

            if (result == null || result.Count() == 0)
            {
                WriteToConsole("No movies found, fix connection with database or load movies to database");
            }
            else
            {
                WriteToConsole("UpdateData finished");
            }

            return result;
        }

        public void UpdateView(string txt)
        {
            WriteToConsole("UpdateView started");
            listViewUsers.ItemsSource = null;

            if (dbMovies == null || dbMovies.Count() == 0)
            {
                WriteToConsole("UpdateView - Movies not found");
                return;
            }

            if (String.IsNullOrEmpty(txt))
            {
                listViewUsers.ItemsSource = dbMovies;
            }
            else
            {
                listViewUsers.ItemsSource = dbMovies?.Where(x =>
                    (!string.IsNullOrEmpty(x.OrginalTitle) && x.OrginalTitle.Contains(txt))
                    || (!string.IsNullOrEmpty(x.OrginalTitle) && x.Title.Contains(txt))
                    || (x.Staff != null && x.Staff.Any(y => y.Name.Contains(txt)))
                    || (x.Types != null && x.Types.Any(z => z.Name.Contains(txt))));
            }
            WriteToConsole("UpdateView finished");
        }

        public void Working(bool state)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                loadingShopB.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
            }));
        }


        public void WriteToConsole(string txt)
        {
            // OUTPUT DO KONSOLI PROGRAMU 
            // UZYTO DISPATCHERA - USTALA ON KOLEJKI WATKOW KTORE CHCE MIEC DOSTEP DO OKRESLONEJ CZESCI KODU
            // W NASZYM PRZYPADKU DO KONTROLKI TEXTBLOCK (ConsoleOut)

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ConsoleOut.Text += "INFO " + DateTime.Now + "                - " + txt + Environment.NewLine;
            }));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Step2.IsEnabled == true || Step3.IsEnabled == true)
            {
                WriteToConsole("Cannot do this operation");
                return;
            }
               
            MessageBoxResult result = MessageBox.Show("Would you like to start ETL process? \nThis operation can't be undone", "Choose you decision !!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (!(Step2.IsEnabled == false && Step3.IsEnabled == false))
                            return;

                        Start.IsEnabled = false;
                        Step1.IsEnabled = false;
                        Step2.IsEnabled = false;
                        Step3.IsEnabled = false;

                        // BACKGROUNDWORKER POZWALA NA WYKONYWANIE OPERACJI W TLE
                        // POZWALA NA WYKONYWANIE ZLOZONYCH OPERACJI BEZ ZAWIESZANIA GUI

                        BackgroundWorker backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                        backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                        backgroundWorker1.RunWorkerAsync();
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) => Extract(true); 

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Transform();
            Working(true);
            LoadInBackground();
            Working(false);
        }

        private void LoadInBackground()
        {
            BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(Load);
            bgw.RunWorkerAsync();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to reset?", "Choose you decision !!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        MessageBox.Show("Reseted Complete");

                        moviesToLoad = null;
                        dbMovies = null;                    
                        listViewUsers.ItemsSource = null;
                        Start.IsEnabled = true;
                        Step1.IsEnabled = true;
                        Step2.IsEnabled = false;
                        Step3.IsEnabled = false;

                        ConsoleOut.Text = DateTime.Now + " Reseted";
                        Search_Async();
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Setp1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to start STEP 1?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (Step2.IsEnabled == true)
                        {
                            MessageBox.Show("Cannot do this operation");
                            WriteToConsole("Cannot run Extract proces Again");
                            return;
                        }

                        Extract();
                        Step2.IsEnabled = true;
                        Start.IsEnabled = false;
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }

        }

        private void Setp2_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Do you want to start STEP 2?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (Step3.IsEnabled == true)
                        {
                            MessageBox.Show("Cannot do this operation");
                            WriteToConsole("Cannot run Transform proces Again");
                            return;
                        }

                        Transform();
                        Step3.IsEnabled = true;

                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Setp3_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to start STEP 3?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        Working(true);
                        LoadInBackground();
                        Working(false);
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }         
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchParameter = searchTB.Text;
            Search_Async(searchParameter, false);
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete all data? \nThis operation will purge all data from DataBase", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        Working(true);
                        WriteToConsole("Delete All started");
                        DataCallers.Instance.RemoveAll();
                        Working(false);

                        WriteToConsole("Delete All finished");
                        Search_Async();
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }

        private object extractLock = new object();
        private void Extract(bool withWait = false)
        {
            WriteToConsole("Scrapping started");

            for (int i = 1; i <= proc; i++)
            { 
                var page = i;
                lock (extractLock)
                {
                    try
                    {
                        tasksArray[i - 1] = new Task(() => Scrap(page));
                        tasksArray[i - 1].Start();
                        WriteToConsole($"Thread : {i} started");
                    }
                    catch (Exception ex)
                    {
                        WriteToConsole("Error with threads");
                        WriteToConsole(ex.Message);
                    }
                }
            }
            if (withWait)
                Task.WaitAll(tasksArray);
        }

        private async Task<bool> Scrap(int page)
        {
            await scraper.ScrapeMovies(page);
            WriteToConsole($"Thread : {page} finished");
            return true;
        }

        private void Transform()
        {
            try
            {
                // METODA TRANSFORMUJACA DANE Z PLIKOW JSON DO OBIEKTOW 
                moviesToLoad = new List<Movie>();
                WriteToConsole("Transform started");
                WriteToConsole("Find json directory");
                string path = null;

                try
                {
                    path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jsons";
                }
                catch (Exception ex)
                {
                    WriteToConsole("Error with directory path");
                    WriteToConsole(ex.Message);
                    return;
                }

                // POBIERAMY PLIKI
                string[] files = Directory.GetFiles(path, "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

                if (files == null || files.Length == 0)
                {
                    WriteToConsole("Error with finding files");
                    return;
                }

                // ITERUJEMY PO PLIKACH
                foreach (var file in files)
                {
                    var movieList = new List<Movie>();
                    var filePath = path + $@"\{file}";

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        WriteToConsole($"Transfrom {file} started");
                        String line = sr.ReadToEnd();
                        dynamic json = JsonConvert.DeserializeObject(line);

                        foreach (var it in json)
                        {
                            // KONWETUJEMY DANE
                            var movie = new Movie
                            {
                                BoxOffice = Converters.ConvertToDecimal(it.BoxOffice),
                                Description = Converters.ConvertToString(it.Description),
                                Director = Converters.ConvertToString(it.Director),
                                Duration = Converters.ConvertToString(it.Duration),
                                ReleaseDate = Converters.ConvertToDateTime(it.ReleaseDate),
                                Rank = Converters.ConvertToInt(it.Rank),
                                Title = Converters.ConvertToString(it.Title),
                                OrginalTitle = Converters.ConvertToString(it.OrginalTitle),
                                Production = Converters.ConvertToProduction(it.Production),
                                Rate = Converters.ConvertToInt(it.Rate),
                                RateTotalVotes = Converters.ConvertToInt(it.RateTotalVotes),
                                Year = Converters.ConvertToInt(it.Year),
                            };

                            movie.Staff = new List<Person>();
                            foreach (var person in it.Staff)
                            {
                                movie.Staff.Add(new Person
                                {
                                    Name = Converters.ConvertToString(person.Name),
                                    Surname = Converters.ConvertToString(person.Surname),
                                });
                            }

                            movie.Types = new List<MovieType>();
                            foreach (var type in it.Types)
                            {
                                movie.Types.Add(new MovieType
                                {
                                    Description = Converters.ConvertToString(type.Description),
                                    Name = Converters.ConvertToString(type.Name),
                                    Type = Converters.ConvertToMovieType(type.Type),

                                });
                            }

                            WriteToConsole($"Transform finished - movie: {movie.Title}, by {movie.Director}");
                            movieList.Add(movie);
                        }
                        WriteToConsole($"Transfrom of {file} finished");
                        moviesToLoad.AddRange(movieList);
                    }

                    // USUWAMY PLIKI 
                    WriteToConsole($"Delete json files: {filePath}");
                    File.Delete(filePath);
                }
            }
            catch (IOException ex)
            {
                WriteToConsole("The file could not be read:");
                WriteToConsole(ex.Message);
                return;
            }
        }

        private void Load(object sender, DoWorkEventArgs e)
        {
            WriteToConsole("Load to data base movies (count) - " + moviesToLoad.Count);
            threadsArray[0] = new Thread(() => DataCallers.Instance.AddMovies(moviesToLoad));
            threadsArray[0].Start(); WriteToConsole("Load to data base started");
            threadsArray[0].Join();

            moviesToLoad = null;

            Search_Async();
        }

        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Do you want to export all data to CSV file?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (dbMovies == null || dbMovies.Count() == 0)
                        {
                            MessageBox.Show("No movies found !! \nTry again");
                            break;
                        }

                        ExportCSV.Serialize(dbMovies);
                        MessageBox.Show($"Completed - file db.csv created");
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
          
        }


        private void ExportCSVOne_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to export selected movie data to CSV file?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        var movie = dbMovies?.Where(x => !string.IsNullOrEmpty(exportId.Text) && x.MovieId == Convert.ToInt32(exportId.Text));

                        if (movie == null || movie.Count() == 0)
                        {
                            MessageBox.Show("Movie not found !! \nTry again");
                            break;
                        }

                        ExportCSV.Serialize(movie, exportId.Text);
                        MessageBox.Show($"Completed - file MovieId-{exportId.Text}.csv created");
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }        
        }


        private void SearchMovieId_Click(object sender, RoutedEventArgs e)
        {
            WriteToConsole("Search movie for edit started ");

            movieToEdit = dbMovies?.Where(x => !string.IsNullOrEmpty(editMovieId.Text) && x.MovieId == Convert.ToInt32(editMovieId.Text)).FirstOrDefault();

            if (movieToEdit == null)
            {
                MessageBox.Show("Movie not found\nTry again");
                WriteToConsole("Search movie - not found");
                return;
            }
               

            titleTb.Text = movieToEdit.Title;
            orginalTitleTb.Text = movieToEdit.OrginalTitle;
            descriptionTb.Text = movieToEdit.Description;
            rankTb.Text = movieToEdit.Rank.ToString();
            rateTb.Text = movieToEdit.Rate.ToString();
            rateTotalTb.Text = movieToEdit.RateTotalVotes?.ToString();
            directorTb.Text = movieToEdit.Director;
            durationTb.Text = movieToEdit.Duration;
            yearTb.Text = movieToEdit.Year?.ToString();
            boxTb.Text = movieToEdit.BoxOffice.ToString();

            WriteToConsole("Search movie for edit finished ");
        }

        private void EditMovieClick(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Choose one option", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (movieToEdit == null) return;

                        WriteToConsole("Edit movie started");

                        movieToEdit.Title = titleTb.Text;
                        movieToEdit.OrginalTitle = orginalTitleTb.Text;
                        movieToEdit.Description = descriptionTb.Text;
                        movieToEdit.Rank = string.IsNullOrEmpty(rankTb.Text) ? (int?)null : Convert.ToInt32(rankTb.Text);
                        movieToEdit.Rate = string.IsNullOrEmpty(rateTb.Text) ? (int?)null : Convert.ToInt32(rateTb.Text);
                        movieToEdit.RateTotalVotes = string.IsNullOrEmpty(rateTotalTb.Text) ? (int?)null : Convert.ToInt32(rateTotalTb.Text);
                        movieToEdit.Director = directorTb.Text;
                        movieToEdit.Duration = durationTb.Text;
                        movieToEdit.Year = string.IsNullOrEmpty(yearTb.Text) ? (int?)null : Convert.ToInt32(yearTb.Text);
                        movieToEdit.BoxOffice = Convert.ToDecimal(boxTb.Text);

                        var saved = DataCallers.Instance.EditMovie(movieToEdit);

                        if (saved)
                        {
                            MessageBox.Show("Completed");
                            WriteToConsole("Edit movie success");
                        }
                        else
                        {
                            MessageBox.Show("Failed");
                            WriteToConsole("Edit movie failed");
                        }

                        Search_Async();
                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}