using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace DAE_Prode.Models
{
    public class topfivemodels
    {
        public  int     id              { get; set; }
        public  int idusuario           { get; set; }
        public  string hiddennombre     { get; set; }
        public  string hiddenapellido   { get; set; }
        public  int puntosporfecha      { get; set; }

        PRODEDataContext db = new PRODEDataContext();
        /* ---------------------------------------------------------------------*/
        public IEnumerable<topfivemodels> Vertopfive()
        { IEnumerable<topfivemodels> ltopfives;
        if (HttpRuntime.Cache["TOPFIVES"] == null)
        {
            ltopfives =
            (from auxt in db.topfives
             join auxu in db.usuarios on auxt.idusuario equals auxu.id
             orderby auxt.puntosporfecha descending
             select new topfivemodels
             {
                 id             = auxt.id,
                 idusuario      = auxt.idusuario,
                 hiddennombre   = auxu.nombre,
                 hiddenapellido = auxu.apellido,
                 puntosporfecha = auxt.puntosporfecha,
             }).Take(5);
            HttpRuntime.Cache.Add("TOPFIVES", ltopfives, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        
        }
        else {
            ltopfives = (IEnumerable<DAE_Prode.Models.topfivemodels>)HttpRuntime.Cache.Get("TOPFIVES");
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

            var lista =
            (from auxt in db.topfives
             select auxt
             );
            db.topfives.DeleteAllOnSubmit(lista);
            db.SubmitChanges();
        }
        /* ---------------------------------------------------------------------*/
    }

}