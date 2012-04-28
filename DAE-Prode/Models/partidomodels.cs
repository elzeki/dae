using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DAE_Prode.Models
{
    public class partidomodels
    {    
        PRODEDataContext db = new DAE_Prode.Models.PRODEDataContext();
        [Required]
        public int      id                  { get; set; }
        [Required]      
        public int      equipolocal         { get; set; }

        public string   hiddenlocal         { get; set; }
          [Required]
        public int      equipovisita        { get; set; }
        
        public string   hiddenvisita        { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "{0}, Rango invalido, Ingrese un valor entre {1}y{2}")]
        [Display(Name = "Goles Local")]
        public int      goleslocal          { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "{0}, Rango invalido, Ingrese un valor entre {1}y{2}")]
        [Display(Name = "Goles Visitante")]
        public int      golesvisita         { get; set; }
        public int      idfechatorneo       { get; set; }
        public string   hiddenfecha         { get; set; }
        public int      jugado              { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Estadio")]
        public string   estadio             { get; set; }
     
     /* -------------------------------------------------------------------------------------------  */
        public partidomodels partidoporfechaelyev(int idf, int el, int ev)
        {
            partidomodels partido = (
              from auxp in db.partidos
              where ((auxp.idfechatorneo == idf) && (auxp.equipolocal == el) && (auxp.equipovisita == ev))
              select new partidomodels{
                goleslocal=auxp.goleslocal,
                golesvisita=auxp.golesvisita,
              }
              ).First();
            return (partido);
        }
    /* -------------------------------------------------------------------------------------------  */
        public IEnumerable<partidomodels> listapartidosporfecha(int id)
        {
            IEnumerable<partidomodels> lpartidos =
            (from aux1fecha in db.fechas

             join aux2partido   in db.partidos  on aux1fecha.id             equals aux2partido.idfechatorneo
             where aux2partido.idfechatorneo==id
             join aux3equipo    in db.equipos   on aux2partido.equipolocal  equals aux3equipo.id
             join aux4equipo    in db.equipos   on aux2partido.equipovisita equals aux4equipo.id
             select new partidomodels
             {
                 id             = aux2partido.id,
                 idfechatorneo  = aux2partido.idfechatorneo,
                 hiddenfecha    = aux1fecha.nombre,
                 equipolocal    = aux2partido.equipolocal,
                 hiddenlocal    = aux3equipo.nombre,
                 hiddenvisita   = aux4equipo.nombre,
                 equipovisita   = aux2partido.equipovisita,
                 goleslocal     = aux2partido.goleslocal,
                 golesvisita    = aux2partido.golesvisita,
                 jugado         = aux2partido.jugado,
                 estadio        = aux2partido.estadio,
             });
            return (lpartidos);
        }
        /* -------------------------------------------------------------------------------------------  */
        public IEnumerable<partidomodels> listapartidosportorneo(int idt)
        {
            IEnumerable<partidomodels> lpartidos =
            (
             from auxp in db.partidos
             join auxf in db.fechas on  auxp.idfechatorneo equals auxf.id
             join auxt in db.torneos on auxf.idtorneo equals auxt.id
             where ((auxt.id == idt)&&(auxp.jugado==1))
             select new partidomodels
             {
                 id             = auxp.id,
                 idfechatorneo  = auxp.idfechatorneo,
                 equipolocal    = auxp.equipolocal,
                 equipovisita   = auxp.equipovisita,
                 goleslocal     = auxp.goleslocal,
                 golesvisita    = auxp.golesvisita,
                 jugado         =auxp.jugado,
                 estadio        = auxp.estadio,
             });
            return (lpartidos);
        }

        /* -------------------------------------------------------------------------------------------  */
        public IEnumerable<partidomodels> listapartidos()
        {     IEnumerable<partidomodels> lpartidos;
        if (HttpRuntime.Cache["PARTIDOS"] == null)
        {
            lpartidos =
            (from aux1fecha in db.fechas
             join aux2partido in db.partidos on aux1fecha.id equals aux2partido.idfechatorneo
             join aux3equipo in db.equipos on aux2partido.equipolocal equals aux3equipo.id
             join aux4equipo in db.equipos on aux2partido.equipovisita equals aux4equipo.id
             select new partidomodels
             {
                 id = aux2partido.id,
                 idfechatorneo  = aux2partido.idfechatorneo,
                 hiddenfecha    = aux1fecha.nombre,
                 equipolocal    = aux2partido.equipolocal,
                 hiddenlocal    = aux3equipo.nombre,
                 hiddenvisita   = aux4equipo.nombre,
                 equipovisita   = aux2partido.equipovisita,
                 goleslocal     = aux2partido.goleslocal,
                 golesvisita    = aux2partido.golesvisita,
                 jugado         = aux2partido.jugado,
                 estadio        = aux2partido.estadio,
             });
        }
        else {
            lpartidos = (IEnumerable<DAE_Prode.Models.partidomodels>)HttpRuntime.Cache.Get("PARTIDOS"); 
        }

            return (lpartidos);
        }
        /* -------------------------------------------------------------------------------------------  */
        public IEnumerable<partidomodels> verpartido(int id)
        { IEnumerable<partidomodels> partido =(
          from aux in db.partidos
          where aux.id==id
          select new partidomodels
             {
                 id             =aux.id,
                 equipolocal    =aux.equipolocal,
                 equipovisita   =aux.equipovisita,
                 goleslocal     =aux.goleslocal,
                 golesvisita    =aux.golesvisita,
                 idfechatorneo  =aux.idfechatorneo,
                 jugado         =aux.jugado,
                 estadio        =aux.estadio,
             });
            return (partido);
        }
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */
    /* -------------------------------------------------------------------------------------------  */

    }
}