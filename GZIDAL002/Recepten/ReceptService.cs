﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GZIDAL002.Helpers;
using GZIDAL002.Medicijnen.Models;
using GZIDAL002.Patienten.Models;
using GZIDAL002.Recepten.Models;
using Newtonsoft.Json;
using static GZIDAL002.Config;

namespace GZIDAL002.Recepten
{
    public class ReceptService
    {
        APIHelper _api;

        public ReceptService()
        {
            _api = new APIHelper();
        }

        public async Task<Recept> AddReceptRegel(
            Recept recept,
            Medicijn medicijn,
            int aantal,
            string dosering
        )
        {
            var url = $"{API_URL}/zi-v0/receptline";
            var data = new Dictionary<string, dynamic>
            {
                { "vesId", recept.Patient.VesId},
                { "patId", recept.Patient.PatId },
                { "prKode", medicijn.PRKode },
                { "aantal", aantal },
                { "dosering", dosering },
                { "recId", recept.Id },
            };

            var response = await _api.Post<AddReceptRegelResponseRoot>(url, data);
            var regel = response.Regel[0];

            if(response.Regel[0].Status[0].StatusCode >= 0)
            {
                recept.Id = regel.Id;
                recept.AddRegel(new ReceptRegel()
                {
                    Medicijn = medicijn,
                    Aantal = aantal,
                    Dosering = dosering,
                    ContraIndicaties = regel.ContraIndicaties,
                    Interacties = regel.Interacties
                });
            }

            return recept;
        }
    }
}
