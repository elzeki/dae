using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace MvcApplication6.Models
{
    public class usuariomodels
    {
        PRODEDataContext db = new MvcApplication6.Models.PRODEDataContext();

        public int id           { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "User Name")]
        public string username  { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 4)]
        [Display(Name = "Password")]
        public string password  { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre    { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Apellido")]
        public string apellido  { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Fecha de Nacimiento")]
        public string fechanac  { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string email     { get; set; }

         /*-----------------------------------------------------------------------------------------------------------*/

        public int vermispuntosportorneo(int idt, int iduser)
        {
            var laux = (
             from auxb in db.boletas
             join auxf in db.fechas on auxb.idfechatorneo equals auxf.id
             where ((auxf.idtorneo == idt)&&(auxb.idusuario==iduser))
             group new { auxb, auxf } by new { auxb.idusuario } into g
            select new boletamodels
            {
                puntostotales = g.Sum(p => p.auxb.puntostotales),
            }
            );
            var total = 0;
            if (laux.Count() > 0)
            {
                total = laux.First().puntostotales;
            }
            return (total);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public int vermispuntosporfecha(int idf, int iduser)
        {

            
            var laux = (
                    from auxb in db.boletas

                    join auxf in db.fechas on auxb.idfechatorneo equals auxf.id
                    where ((auxb.idusuario==iduser) && (auxf.id == idf))
                    group new { auxb, auxf } by new { auxb.idusuario } into g

                    select new 
                    {
                        
                        puntostotales = g.Sum(p => p.auxb.puntostotales),
                    }
                );
            var total=0;
            if (laux.Count() > 0)
            {  total=laux.First().puntostotales;
            }
            return (total);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<usuariomodels> listausuarios()
        {
            IEnumerable<usuariomodels> lusuarios;
            if (HttpRuntime.Cache["USUARIOS"] == null)
            {
                    lusuarios =
                    (from aux in db.usuarios
                     select new usuariomodels
                     {
                         id = aux.id,
                         nombre = aux.nombre,
                         username = aux.username,
                         password = aux.password,
                         apellido = aux.apellido,
                         fechanac = aux.fechanac,
                         email = aux.email,

                     });
                HttpRuntime.Cache.Add("USUARIOS", lusuarios, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        
            }

            else {
                lusuarios = (IEnumerable<MvcApplication6.Models.usuariomodels>)HttpRuntime.Cache.Get("USUARIOS");
            }
            return (lusuarios);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
        public IEnumerable<usuariomodels> verusuario(int id)
        {
            IEnumerable<usuariomodels> lusuario =
            (from aux in db.usuarios
             where aux.id == id
             select new usuariomodels
             {
                 id = aux.id,
                 nombre = aux.nombre,
                 username = aux.username,
                 password = aux.password,
                 apellido = aux.apellido,
                 fechanac = aux.fechanac,
                 email = aux.email,
             });
            return (lusuario);
        }
        /*-----------------------------------------------------------------------------------------------------------*/
    }
}