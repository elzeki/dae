using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MvcApplication6.Models
{
    public class topfivemodels
    {
        int     id { get; set; }		
	    int     idusuario { get; set; }
        int     puntosporfecha{ get; set; }

        PRODEDataContext db = new PRODEDataContext();
        /* ---------------------------------------------------------------------*/
        public IEnumerable<topfivemodels> Vertopfive()
        { IEnumerable<topfivemodels> ltopfives;
        if (HttpRuntime.Cache["TOPFIVES"] == null)
        {
            ltopfives =
            (from auxt in db.topfives
             select new topfivemodels
             {
                 id = auxt.id,
                 idusuario = auxt.idusuario,
                 puntosporfecha = auxt.puntosporfecha,
             });
            HttpRuntime.Cache.Add("TOPFIVES", ltopfives, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        
        }
        else {
            ltopfives = (IEnumerable<MvcApplication6.Models.topfivemodels>)HttpRuntime.Cache.Get("TOPFIVES");
        }
        return (ltopfives);
        }
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