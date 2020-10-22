﻿using System;
using System.Diagnostics;
using Xamarin.Forms;
using GZIDAL002.Patienten.Models;
using System.Threading.Tasks;
using medicijn.Utils;
using System.Windows.Input;
using medicijn.Views.Recepten;
using medicijn.Views.Patienten;
using GZIDAL002.Patienten;

namespace medicijn.ViewModels.Patienten
{
    public class ViewPatientViewModel : BaseViewModel 
    {
        PatientService _patientSerivce;

        public ICommand CloseOverlayCommand { get; }
        public ICommand ShowPatientMedicationPressedCommand { get; }
        public ICommand CreateNewReceptPressedCommand { get; }
        public ICommand GCButtonPressedCommand { get; }

        public Patient Patient { get; set; }

        public ViewPatientViewModel()
        {
            _patientSerivce = new PatientService();

            CloseOverlayCommand = new Command(CloseOverlay);
            ShowPatientMedicationPressedCommand = new Command(NavigateToPatientMedication);
            CreateNewReceptPressedCommand = new Command(NavigateToCreateNewRecept);
            GCButtonPressedCommand = new Command(CleanPatientData);
        }

        public ViewPatientViewModel(Patient patient) : this()
        {
            Patient = patient;
        }

        public async void CleanPatientData()
        {
            var success = await _patientSerivce.GCPatient(Patient);

            if (!success)
                Debug.WriteLine("FAILED Garbage Collection");
        }

        public void NavigateToCreateNewRecept()
        {
            Navigator.Instance.Add(new MakeReceptView(Patient));
        }

        public void NavigateToPatientMedication()
        {
            Navigator.Instance.Add(new ViewPatientMedicatieView(Patient));
        }

        public void CloseOverlay()
        { 
            Modal.Instance.IsVisible = false;
        }
    }
}
    