using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DAE_Prode.Models
{   
    public class boletamodels
    {
        PRODEDataContext db = new PRODEDataContext();
        [Required]
        public int      id              { get; set; }
        [Required]
        public int      idusuario       { get; set; }
        [Required]
        [Display(Name = "Equiopo Local")]
        public int      equipolocal     { get; set; }
        [Required]
        [Display(Name = "Equiopo Visitante")]
        public int      equipovisita    { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "{0}, Rango invalido, Ingrese un valor entre {1}y{2}")]
        [Display(Name = "Goles Local")]
        public int      goleslocal      { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "{0}, Rango invalido, Ingrese un valor entre {1}y{2}")]
        [Display(Name = "Goles Visitante")]
        public int      golesvisita     { get; set; }
        [Required]
        public int      idfechatorneo   { get; set; }
        [Required]
        [Range(0, 900, ErrorMessage = "{0}, Rango invalido, Ingrese un valor entre {1}y{2}")]
        [Display(Name = "Puntos Totales")]
        public int      puntostotales   { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 1)]
        [Display(Name = "Estadio")]
        public string estadio { get; set; }
        public int editable { get; set; }
 /* --------------------------------------------------------------------------------------  */
        public Boolean dospuntos (int bgl,int  bgv, int pgl,int pgv)
        { return ((bgl==pgl)&&(bgv==pgv));
        }
        /* --------------------------------------------------------------------------------------  */
        public Boolean unpunto(int bgl, int bgv, int pgl, int pgv)
        {
            return (
              (         (bgl > bgv) && (pgl > pgv)          )
           || (         (bgl < bgv) && (pgl < pgv)          )
           || (         (bgl == bgv) && (pgl == pgv)        )
            
                
                
            );
        }
        /* --------------------------------------------------------------------------------------  */
         public void meterpuntosboleta(int idf)
         {
             IEnumerable<boletamodels> lboletas = (
               from auxb in db.boletas
               where auxb.idfechatorneo == idf
               select new boletamodels {
                   id = auxb.id,
                   idusuario = auxb.idusuario,
                   idfechatorneo = auxb.idfechatorneo,
                   equipolocal = auxb.equipolocal,
                   equipovisita = auxb.equipovisita,
                   goleslocal = auxb.goleslocal,
                   golesvisita = auxb.golesvisita,
                   puntostotales = auxb.puntostotales,
                   estadio = auxb.estadio,
                   editable = auxb.editable,
               }
               );
             int total;
             foreach (var auxb in lboletas)
             {
                 var partido = new partidomodels().partidoporfechaelyev(idf, auxb.equipolocal, auxb.equipovisita);
                 if (dospuntos(auxb.goleslocal, auxb.golesvisita, partido.goleslocal, partido.golesvisita))
                 {
                     total = 2;
                 }
                 else if (unpunto(auxb.goleslocal, auxb.golesvisita, partido.goleslocal, partido.golesvisita))
                 {
                     total = 1;
                 }
                 else total = 0;
                 if (total != 0)
                 {
                     auxb.puntostotales = total;
                      /*-------------------------------------------------------- */
                     saveboleta(auxb);

                     /*-------------------------------------------------------- */  

                 }
             }
         }



         /* --------------------------------------------------------------------------------------  */
       public void saveboleta (boletamodels auxb)
        {
                     boleta T = new boleta(); 
                     T = db.boletas.Single(q => q.id == auxb.id);
                     T.idusuario = auxb.idusuario;
                     T.equipolocal = auxb.equipolocal;
                     T.equipovisita = auxb.equipovisita;
                     T.goleslocal = auxb.goleslocal;
                     T.golesvisita = auxb.golesvisita;
                     T.idfechatorneo = auxb.idfechatorneo;
                     T.estadio = auxb.estadio;
                     T.puntostotales = auxb.puntostotales;
                     T.editable = auxb.editable;
                     db.SubmitChanges();
        }
        /* --------------------------------------------------------------------------------------  */
        public IEnumerable<boletamodels> boletaporfechaporuser(int id, int idUser)
        {
            IEnumerable<boletamodels> lboletas =
            
           (from aux1boleta in db.boletas
            where ((aux1boleta.idfechatorneo == id) && (aux1boleta.idusuario == idUser))
            select new boletamodels
           {
                id = aux1boleta.id,
                idusuario = idUser,
                idfechatorneo = aux1boleta.idfechatorneo,
                equipolocal = aux1boleta.equipolocal,
                equipovisita = aux1boleta.equipovisita,
                goleslocal = aux1boleta.goleslocal,
                golesvisita = aux1boleta.golesvisita,
                puntostotales = aux1boleta.puntostotales,
                estadio = aux1boleta.estadio,
                editable=aux1boleta.editable,
            });

            return (lboletas);
        }
        /* --------------------------------------------------------------------------------------  */
        public IEnumerable<boletamodels> boletaporfecha(int idf)
        {
            IEnumerable<boletamodels> lboletas =
           (from aux1boleta in db.boletas
            where aux1boleta.idfechatorneo == idf
            select new boletamodels
            {
                id              = aux1boleta.id,
                idusuario       = aux1boleta.idusuario,
                idfechatorneo   = aux1boleta.idfechatorneo,
                equipolocal     = aux1boleta.equipolocal,
                equipovisita    = aux1boleta.equipovisita,
                goleslocal      = aux1boleta.goleslocal,
                golesvisita     = aux1boleta.golesvisita,
                puntostotales   = aux1boleta.puntostotales,
                estadio         = aux1boleta.estadio,
                editable        = aux1boleta.editable,
            });
            return (lboletas);
        }
        /* --------------------------------------------------------------------------------------  */
        /*public IEnumerable<usuariomodels>  lusuarios (int idf){ 
            IEnumerable<usuariomodels>   lusuarios= (
            from auxb in  db.boletas  
            where auxb.idfechatorneo == idf
            select new usuariomodels{
            id=auxb.idusuario,
            }).Distinct();
            return (lusuarios);
        }
         * */
        /* --------------------------------------------------------------------------------------  */
        /* me devuelve los ususarios pertenecientes a las boletas con la suma de sus puntos por torneo.*/
        public IEnumerable <boletamodels> boletasxtorneoxpuntos(int idtorneo)
        {
            var laux = (
                from auxb in db.boletas
                join auxf in db.fechas on auxb.idfechatorneo equals auxf.id
                where auxf.idtorneo == idtorneo
                group new {auxb, auxf} by new { auxb.idusuario} into g

                select new boletamodels
                {   idusuario=g.Key.idusuario,
                    puntostotales = g.Sum(p => p.auxb.puntostotales), 
                }
            );
            return (laux);
       }
        /*----------------------------------------------------------------------------------------------------- */
        /* me devuelve los ususarios pertenecientes a las boletas con la suma de sus puntos por fecha.*/
        public IEnumerable<boletamodels> boletasxfechaxpuntos(int idfecha)
        {
            var laux = (
                from auxb in db.boletas
                join auxf in db.fechas on auxb.idfechatorneo equals auxf.id
                where auxf.id == idfecha
                group new { auxb, auxf } by new { auxb.idusuario } into g
                select new boletamodels
                {
                    idusuario = g.Key.idusuario,
                    puntostotales = g.Sum(p => p.auxb.puntostotales),
                }
            );
            return (laux);
        }
    }
}