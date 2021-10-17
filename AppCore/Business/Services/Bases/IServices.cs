using AppCore.Business.Models.Results;
using AppCore.Records.Bases;
using System;
using System.Linq;

namespace AppCore.Business.Services.Bases
{


    /*
     Servisin amacı :
     1) Uygulama katmanından bir model alacak bunu entityse dönüştürüp veritabanı işlemi yapıcak.
     2) db den entitiy alıcak modele dönüştürüp yukarı katmana gönderecek.
     */
    public interface IServices<TModel>: IDisposable where TModel: RecordBase , new()
    {
        IQueryable<TModel> GetQuery();
        Result Add(TModel model);
        Result Update(TModel model);
        Result Delete(int id);
    }
}
