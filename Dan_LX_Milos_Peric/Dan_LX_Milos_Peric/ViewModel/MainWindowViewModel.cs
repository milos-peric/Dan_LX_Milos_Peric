using Dan_LX_Milos_Peric.Command;
using Dan_LX_Milos_Peric.Service;
using Dan_LX_Milos_Peric.Utility;
using Dan_LX_Milos_Peric.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dan_LX_Milos_Peric.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow _main;
        DataBaseService _dataBaseService = new DataBaseService();
        List<vwLocation> LocationListFromDB = new List<vwLocation>();
        List<tblLocation> LocationListFromFile = new List<tblLocation>();
        ActionEvent actionEventObject;
        BackgroundWorker backgroundWorker1;

        public MainWindowViewModel(MainWindow mainOpen)
        {
            _main = mainOpen;
            WorkerList = _dataBaseService.GetAllWorkerRecords().ToList();
            LocationListFromDB = _dataBaseService.GetAllLocations();
            if (LocationListFromDB.Count == 0)
            {
                LocationListFromFile = LocationLoader.LoadLocations();
                _dataBaseService.AddLocationsToDataBase(LocationListFromFile);
            }
            actionEventObject = new ActionEvent();
            backgroundWorker1 = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            backgroundWorker1.DoWork += DoWork;
            backgroundWorker1.RunWorkerAsync();
            actionEventObject.ActionPerformed += ActionPerformed;
        }

        #region Properties
        private vwWorker _worker;
        public vwWorker Worker
        {
            get
            {
                return _worker;
            }
            set
            {
                _worker = value;
                OnPropertyChanged("Worker");
            }
        }

        private List<vwWorker> _workerList;
        public List<vwWorker> WorkerList
        {
            get
            {
                return _workerList;
            }
            set
            {
                _workerList = value;
                OnPropertyChanged("WorkerList");
            }
        }




        #endregion

        private ICommand _addNewWorker;
        public ICommand AddNewWorker
        {
            get
            {
                if (_addNewWorker == null)
                {
                    _addNewWorker = new RelayCommand(param => AddNewWorkerExecute(), param => CanAddNewWorkerExecute());
                }
                return _addNewWorker;
            }
        }

        private void AddNewWorkerExecute()
        {
            try
            {
                AddWorker addWorker = new AddWorker();
                addWorker.ShowDialog();
                if ((addWorker.DataContext as AddWorkerViewModel).IsUpdateWorker == true)
                {
                    WorkerList = _dataBaseService.GetAllWorkerRecords().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddNewWorkerExecute()
        {
            return true;
        }

        private ICommand _editWorker;
        public ICommand EditWorker
        {
            get
            {
                if (_editWorker == null)
                {
                    _editWorker = new RelayCommand(param => EditWorkerExecute(), param => CanEditWorkerExecute());
                }
                return _editWorker;
            }
        }

        private void EditWorkerExecute()
        {
            try
            {
                if (Worker != null)
                {
                    EditWorker editWorker = new EditWorker(Worker);
                    editWorker.ShowDialog();
                    WorkerList = _dataBaseService.GetAllWorkerRecords().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanEditWorkerExecute()
        {
            if (Worker == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ICommand _deleteWorker;
        public ICommand DeleteWorker
        {
            get
            {
                if (_deleteWorker == null)
                {
                    _deleteWorker = new RelayCommand(param => DeleteWorkerExecute(), param => CanDeleteWorkerExecute());
                }
                return _deleteWorker;
            }
        }

        private void DeleteWorkerExecute()
        {
            try
            {
                if (Worker != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete employee record?", "Delete Record", MessageBoxButton.OKCancel);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            int _workerID = _worker.WorkerID;
                            _dataBaseService.DeleteWorker(_workerID);
                            string logMessage = string.Format("Worker {0} {1} - JMBG:{2}, was deleted from database.", _worker.FirstName,
                                _worker.LastName, _worker.JMBG);
                            actionEventObject.OnActionPerformed(logMessage);
                            WorkerList = _dataBaseService.GetAllWorkerRecords().ToList();
                            MessageBox.Show("Record deleted!", "Delete Record");
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteWorkerExecute()
        {
            if (Worker == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!Logger.logMessage.Equals(""))
                {
                    Logger.LogToFile();
                    Debug.WriteLine("Action was logged to file Log.txt");
                }
                Thread.Sleep(2000);

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

            }
        }

        void ActionPerformed(object source, ActionEventArgs args)
        {
            Logger.logMessage = args.LogMessage;
        }
    }
}
