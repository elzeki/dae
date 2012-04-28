using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;


namespace DAE_Prode.Models
{
    public class toptenmodels
    {   PRODEDataContext db = new PRODEDataContext();
        public int id               { get; set; }
        public int idusuario        { get; set; }
        public string hiddennombre  { get; set; }
        public string hiddenapellido{ get; set; }
        public int puntosportorneo  { get; set; }
        /* ---------------------------------------------------------------------*/
        public IEnumerable<toptenmodels> Vertopten()
        {
            IEnumerable<toptenmodels> ltoptens;
            if (HttpRuntime.Cache["TOPTENS"] == null)
            {
                ltoptens =
                (from auxt in db.toptens
                 join auxu in db.usuarios on auxt.idusuario equals auxu.id
                 orderby auxt.puntosportorneo descending
                 select new toptenmodels
                 {
                     id                 = auxt.id,
                     idusuario          = auxt.idusuario,
                     hiddennombre       = auxu.nombre,
                     hiddenapellido     = auxu.apellido,
                     puntosportorneo    = auxt.puntosportorneo,
                 }).Take(10);
                HttpRuntime.Cache.Add("TOPTENS", ltoptens, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

            }
            else
            {
                ltoptens = (IEnumerable<DAE_Prode.Models.toptenmodels>)HttpRuntime.Cache.Get("TOPTENS");
            }
            return (ltoptens);
        }
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
           
            var lista =
            (from auxt in db.toptens
             select auxt
             );
            db.toptens.DeleteAllOnSubmit(lista);
            db.SubmitChanges();
        }
        /* ---------------------------------------------------------------------*/
    }

}