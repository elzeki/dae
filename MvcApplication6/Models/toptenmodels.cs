using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;


namespace MvcApplication6.Models
{
    public class toptenmodels
    {   PRODEDataContext db = new PRODEDataContext();
        public int id               { get; set; }
        public int idusuario        { get; set; }
        public int puntosportorneo  { get; set; }
        /* ---------------------------------------------------------------------*/
        public IEnumerable<toptenmodels> Vertopten()
        {
            IEnumerable<toptenmodels> ltoptens;
            if (HttpRuntime.Cache["TOPTENS"] == null)
            {
                ltoptens =
                (from auxt in db.toptens
                 select new toptenmodels
                 {
                     id                 = auxt.id,
                     idusuario          = auxt.idusuario,
                     puntosportorneo    = auxt.puntosportorneo,
                 });
                HttpRuntime.Cache.Add("TOPTENS", ltoptens, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

            }
            else
            {
                ltoptens = (IEnumerable<MvcApplication6.Models.toptenmodels>)HttpRuntime.Cache.Get("TOPTENS");
            }
            return (ltoptens);
        }
        /* ---------------------------------------------------------------------*/
        /* ---------------------------------------------------------------------*/
        public void insertarpuntos( IEnumerable<boletamodels> lboletas )
        {
            foreach (var auxb in lboletas)
            {
                topten P = new topten();
                P.idusuario = auxb.idusuario;
                P.puntosportorneo = auxb.puntostotales;
                db.toptens.InsertOnSubmit(P);
                db.SubmitChanges();
            }
        }
        /* ---------------------------------------------------------------------*/
        public void eliminartabla()
        {
            if (db.toptens.Count() > 0)
            {
                IEnumerable<topten> lista =

                (from aux in db.toptens
                 select new topten
                 {
                     id                 = aux.id,
                     idusuario          = aux.idusuario,
                     puntosportorneo    = aux.puntosportorneo,
                 }
                );
                foreach (var aux in lista)
                {
                    MvcApplication6.Models.topten M = new topten
                    {
                        id = aux.id,
                        idusuario = aux.idusuario,
                        puntosportorneo = aux.puntosportorneo,
                    };
                    db.toptens.Attach(M);
                    db.toptens.DeleteOnSubmit(M);
                    db.SubmitChanges();


                }
            }
        }
        /* ---------------------------------------------------------------------*/
    }

}