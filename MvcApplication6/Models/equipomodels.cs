using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication6.Models
{
    public class equipomodels
    {   PRODEDataContext db = new MvcApplication6.Models.PRODEDataContext();
        
        public int id        { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }


     /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<equipomodels> listaequipos()
        {
            IEnumerable<equipomodels> lequipos =
            (from aux in db.equipos
             orderby aux.nombre
             select new equipomodels
             {
                 id     = aux.id,
                 nombre = aux.nombre
             });
           return (lequipos);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<equipomodels> verequipo(int id)
        {   IEnumerable<equipomodels> equipo =
            (from aux in db.equipos
             where aux.id==id
             select new equipomodels
             {
                 id     = aux.id,
                 nombre = aux.nombre
             });
            return (equipo);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
    }
}