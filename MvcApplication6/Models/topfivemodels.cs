using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication6.Models
{
    public class topfivemodels
    {
        int     id { get; set; }		
	    int     idusuario { get; set; }
        int     puntosporfecha{ get; set; }

        PRODEDataContext db = new PRODEDataContext();

        /* ---------------------------------------------------------------------*/
        /* ---------------------------------------------------------------------*/
        public void insertarpuntos( IEnumerable<boletamodels> lboletas )
        {
            foreach (var auxb in lboletas)
            {
                topfive P = new topfive();
                P.idusuario = auxb.idusuario;
                P.puntosporfecha = auxb.puntostotales;
                db.topfives.InsertOnSubmit(P);
                db.SubmitChanges();
            }
        
            
            
            
            /* var lboletas = new boletamodels().boletaporfechaportorneoporuser(idf, iduser);
            var total = 0;
            foreach (var aux in lboletas)
            {
                total=total + aux.puntostotales; 
            }

            topfive P = new topfive();
            P.idusuario = iduser;
            P.puntosporfecha = total;
            db.topfives.InsertOnSubmit(P);
            db.SubmitChanges();
             * */
        }
        /* ---------------------------------------------------------------------*/
        public void eliminartabla()
        {
            if (db.topfives != null)
            {   IEnumerable<topfivemodels> lista =

                (from aux in db.topfives
                 select new topfivemodels{ 
                    id=aux.id,
                    idusuario=aux.idusuario,
                    puntosporfecha=aux.puntosporfecha,

                 }
                );
        
                foreach (var aux in lista)
                {
                    MvcApplication6.Models.topfive M = new topfive
                        {
                            id = aux.id,
                            idusuario = aux.idusuario,
                            puntosporfecha=aux.puntosporfecha,
                        };
                    db.topfives.Attach(M);
                    db.topfives.DeleteOnSubmit(M);
                    db.SubmitChanges();
                
            
                }
            }
        }
        /* ---------------------------------------------------------------------*/
    }

}