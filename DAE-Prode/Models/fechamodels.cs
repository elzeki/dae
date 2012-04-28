using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DAE_Prode.Models
{
    public class fechamodels
    {
        PRODEDataContext db = new DAE_Prode.Models.PRODEDataContext();

        public int id               { get; set; }
        public int idtorneo         { get; set; }
        public string hiddentorneo  { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre        { get; set; }
        public string fecha1         { get; set; }
        /*----------------------------------------------------------------------------------------------- */
        public IEnumerable<fechamodels> listafechas()
        {
              IEnumerable<fechamodels> lfechas;
              if (HttpRuntime.Cache["FECHAS"] == null)
              {
                  lfechas =
                 (from aux1 in db.fechas
                  join aux2 in db.torneos on aux1.idtorneo equals aux2.id
                  orderby aux1.nombre
                  select new fechamodels
                  {
                      id = aux1.id,
                      idtorneo = aux1.idtorneo,
                      hiddentorneo = aux2.nombre,
                      nombre = aux1.nombre,
                      fecha1 = aux1.fecha1,

                  });
                  HttpRuntime.Cache.Add("FECHAS", lfechas, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        
              }
              else {
                   lfechas = (IEnumerable<DAE_Prode.Models.fechamodels>)HttpRuntime.Cache.Get("FECHAS");
              }

            return (lfechas);
        }
        /*----------------------------------------------------------------------------------------------- */
        public IEnumerable<fechamodels> listafechasportorneo(int id)
        {
            IEnumerable<fechamodels> lfechas =
            (from aux1 in db.fechas
             join aux2 in db.torneos on aux1.idtorneo equals aux2.id
             orderby aux1.nombre
             where aux2.id==id
             select new fechamodels
             {
                 id = aux1.id,
                 idtorneo = aux1.idtorneo,
                 hiddentorneo = aux2.nombre,
                 nombre = aux1.nombre,
                 fecha1 = aux1.fecha1,

             });
            return (lfechas);
        }
        /*----------------------------------------------------------------------------------------------- */
        public IEnumerable<fechamodels> verfecha(int id)
        {
            IEnumerable<fechamodels> fecha =
            (from aux in db.fechas
             where aux.id == id
             select new fechamodels
             {
                 id = aux.id,
                 idtorneo = aux.idtorneo,
                 nombre = aux.nombre,
                 fecha1 = aux.fecha1,
                 
             });
            return (fecha);
        }
    }
    
}