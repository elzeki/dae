using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace DAE_Prode.Models
{
    public class tablageneralmodels
    {
        PRODEDataContext db = new PRODEDataContext();
        public int id                       { get; set; }
        public int idequipo                 { get; set; }
        public string hiddennombreequipo    { get; set; }
        public int puntos                   { get; set; }
        /* ------------------------------------------*/
        public void eliminartabla()
        {  
        var  lista =
        (from auxt in db.tablagenerals
         select  auxt
         );
        db.tablagenerals.DeleteAllOnSubmit(lista);
        db.SubmitChanges();
        }
        /* ------------------------------------------*/
        public IEnumerable<tablageneralmodels> Vertablageneral()
        {
             IEnumerable<tablageneralmodels> lequipos;        
             if (HttpRuntime.Cache["TABLAGENERAL"] == null)
             {
                 lequipos =
                 (    from auxt in db.tablagenerals
                      join auxe in db.equipos on auxt.idequipo equals auxe.id
                      //orderby auxt.puntos descending
                      group new { auxt, auxe } by new { auxe.nombre } into g
                      select new tablageneralmodels
                      {  puntos             = g.Sum(p => p.auxt.puntos),
                         hiddennombreequipo = g.Key.nombre,
                      }
                  );
                   HttpRuntime.Cache.Add("TABLAGENERAL", lequipos, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

             }
            else
            {
                lequipos = (IEnumerable<DAE_Prode.Models.tablageneralmodels>)HttpRuntime.Cache.Get("TABLAGENERAL");
            }
            return (lequipos.OrderByDescending(apuntos => apuntos.puntos)); 
        }
        /* ------------------------------------------*/
    
    }

}