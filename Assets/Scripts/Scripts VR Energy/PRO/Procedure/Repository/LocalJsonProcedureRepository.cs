using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public class LocalJsonProcedureRepository : IProcedureRepository
    {
        private const string ProcedureListJsonPropertyName = "procedures";
        private const string ProcedureJsonFile = "procedures.json";
        
        public Procedure GetProcedure(int id)
        {
            IEnumerable<Procedure> procedures = ListProcedures();
            return procedures.FirstOrDefault(procedure => procedure.Id == id);
        }

        public IEnumerable<Procedure> ListProcedures()
        {
            IEnumerable<Procedure> procedures = null;
            
            TextAsset textAsset = Resources.Load<TextAsset>("Database/Procedure/procedures");
            string json = textAsset.text;
            
            procedures = JObject.Parse(json)[ProcedureListJsonPropertyName].ToObject<IEnumerable<Procedure>>();

            return procedures;
        }

        private string GetProcedureJsonFilePath()
        {
            Debug.Log(Path.Combine(LocalJsonSettings.GetDatabasePath(), nameof(Procedure), ProcedureJsonFile));
            return Path.Combine(LocalJsonSettings.GetDatabasePath(), nameof(Procedure), ProcedureJsonFile);
        }
    }
}
