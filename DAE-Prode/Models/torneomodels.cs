using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DAE_Prode.Models
{   
    public class torneomodels
    {
        PRODEDataContext db = new DAE_Prode.Models.PRODEDataContext();

        public int id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        /*-----------------------------------------------------------------------------------------------------------*/
        public int Verpuntosportorneo(int id) //se obtiene a partir de una id y de un usuario
        {
            var total = 0;
            IEnumerable<boletamodels> lboletas =
            (from auxb in db.boletas
             join auxf in db.fechas on auxb.idfechatorneo equals auxf.id
             join auxt in db.torneos on auxf.idtorneo equals auxt.id
             join auxu in db.usuarios on auxb.idusuario equals auxu.id
             where auxu.id ==1
             select new boletamodels
             { puntostotales=auxb.puntostotales
             }
             );

            foreach (var aux in lboletas)
            {
                total = total + aux.puntostotales;
            }
            return (total);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<torneomodels> listatorneos()
        {
             IEnumerable<torneomodels> ltorneos;
             if (HttpRuntime.Cache["TORNEOS"] == null)
             {
                ltorneos =
                (from aux in db.torneos
                orderby aux.nombre
                select new torneomodels
                {
                    id = aux.id,
                    nombre = aux.nombre
                });
                HttpRuntime.Cache.Add("TORNEOS", ltorneos, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        
             }
             else {
                 ltorneos = (IEnumerable<DAE_Prode.Models.torneomodels>)HttpRuntime.Cache.Get("TORNEOS");
             }
           return (ltorneos);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<torneomodels> vertorneo(int id)
        {   IEnumerable<torneomodels> ltorneo =
            (from aux in db.torneos
             where aux.id==id
             select new torneomodels
             {
                 id = aux.id,
                 nombre = aux.nombre
             });
            return (ltorneo);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
    }
}