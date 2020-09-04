using Dan_LX_Milos_Peric.Command;
using Dan_LX_Milos_Peric.Service;
using Dan_LX_Milos_Peric.Utility;
using Dan_LX_Milos_Peric.Validation;
using Dan_LX_Milos_Peric.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dan_LX_Milos_Peric.ViewModel
{
    class AddWorkerViewModel : ViewModelBase
    {
        AddWorker _addWorker;
        DataBaseService _dbService = new DataBaseService();
        ActionEvent actionEventObject;

        public AddWorkerViewModel(AddWorker addEmployeeOpen)
        {
            _addWorker = addEmployeeOpen;
            _worker = new vwWorker();
            _location = new vwLocation();
            LocationList = _dbService.GetAllLocations();
            WorkerList = _dbService.GetAllWorkerRecords();
            GenderList = _dbService.GetAllGenders();
            SectorList = _dbService.GetAllSectors();
            ManagerList = _dbService.GetAllManagers();
            actionEventObject = new ActionEvent();
            actionEventObject.ActionPerformed += ActionPerformed;
        }

        //private DateTime _startDate = DateTime.Now;
        //public DateTime StartDate
        //{
        //    get { return _startDate; }
        //    set { _startDate = value; OnPropertyChanged("StartDate"); }
        //}

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

        private tblGender _gender;
        public tblGender Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private tblSector _sector;
        public tblSector Sector
        {
            get
            {
                return _sector;
            }
            set
            {
                _sector = value;
                OnPropertyChanged("Sector");
            }
        }

        private vwLocation _location;
        public vwLocation Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        private vwManager _manager;
        public vwManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager = value;
                OnPropertyChanged("Manager");
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

        private List<tblGender> _genderList;
        public List<tblGender> GenderList
        {
            get
            {
                return _genderList;
            }
            set
            {
                _genderList = value;
                OnPropertyChanged("GenderList");
            }
        }

        private List<tblSector> _sectorList;
        public List<tblSector> SectorList
        {
            get
            {
                return _sectorList;
            }
            set
            {
                _sectorList = value;
                OnPropertyChanged("SectorList");
            }
        }

        private List<vwLocation> _locationList;
        public List<vwLocation> LocationList
        {
            get
            {
                return _locationList;
            }
            set
            {
                _locationList = value;
                OnPropertyChanged("LocationList");
            }
        }

        private List<vwManager> _managerList;
        public List<vwManager> ManagerList
        {
            get
            {
                return _managerList;
            }
            set
            {
                _managerList = value;
                OnPropertyChanged("ManagerList");
            }
        }
        #endregion

        private bool _isUpdateWorker;
        public bool IsUpdateWorker
        {
            get
            {
                return _isUpdateWorker;
            }
            set
            {
                _isUpdateWorker = value;
            }
        }

        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        private void SaveExecute()
        {
            try
            {
                if (EntryValidation.ValidateName(Worker.FirstName) == false)
                {
                    MessageBox.Show("First Name can only contain letters. Please try again", "Invalid input");
                    return;
                }
                if (EntryValidation.ValidateName(Worker.LastName) == false)
                {
                    MessageBox.Show("Last Name can only contain letters. Please try again", "Invalid input");
                    return;
                }
                if (EntryValidation.ValidateDate(Worker.DateOfBirth) == false)
                {
                    MessageBox.Show("Person Must be at least 16 years of age. Please try again", "Invalid input");
                    return;
                }
                if (EntryValidation.ValidatePersonalIDNumber(Worker.PersonalIDNumber) == false)
                {
                    MessageBox.Show("Personal ID nubmer you entered is not valid. Please try again", "Invalid input");
                    return;
                }
                if (EntryValidation.ValidateJmbg(Worker.JMBG) == false)
                {
                    MessageBox.Show("JMBG you entered is not valid. Please try again", "Invalid input");
                    return;
                }
                if (EntryValidation.ValidatePhone(Worker.PhoneNumber) == false)
                {
                    MessageBox.Show("Phone number you entered is not in correct format. Please try again", "Invalid input");
                    return;
                }
                Worker.GenderID = Gender.GenderID;
                Worker.SectorID = Sector.SectorID;
                if (Manager != null)
                {
                    Worker.ManagerID = Manager.WorkerID;
                }

                Worker.LocationID = Location.LocationID;
                _dbService.AddWorker(Worker);
                IsUpdateWorker = true;
                string logMessage = string.Format("Worker {0} {1} - JMBG:{2}, was added to database.", _worker.FirstName,
                                _worker.LastName, _worker.JMBG);
                actionEventObject.OnActionPerformed(logMessage);
                MessageBox.Show("New Worker Added Successfully!", "Info");
                _addWorker.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveExecute()
        {
            if (string.IsNullOrEmpty(_worker.FirstName) || string.IsNullOrEmpty(_worker.LastName) ||
                string.IsNullOrEmpty(_worker.JMBG) || string.IsNullOrEmpty(_worker.PersonalIDNumber) ||
                string.IsNullOrEmpty(_worker.PhoneNumber) || string.IsNullOrEmpty(Location.FullLocation) ||
                string.IsNullOrEmpty(Sector.SectorName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ICommand _close;
        public ICommand Close
        {
            get
            {
                if (_close == null)
                {
                    _close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return _close;
            }
        }

        private void CloseExecute()
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close window?", "Close Window", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        _addWorker.Close();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        void ActionPerformed(object source, ActionEventArgs args)
        {
            Logger.logMessage = args.LogMessage;
        }
    }
}
