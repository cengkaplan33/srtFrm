using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surat.Base.Model.Entities;
using Surat.Entities;
using KonsolideRapor.Base.Manage;
using KonsolideRapor.Base.Model;
using System.Data.Entity;
namespace KonsolideRapor.Base.Repositories
{
    public class PaymentCollectingRepository:GenericRepository<PaymentCollecting>
    {

        #region Constructor
        //Hangi context kullanılacaksa burada(constructor da) belirtiyoruz. 
        public PaymentCollectingRepository(KonsolideRaporContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }

        #endregion

        #region Private Members

        private KonsolideRaporContext context;

        #endregion

        #region Public Members

        public KonsolideRaporContext Context
        {
            get { return context; }
        }

        #endregion
    }
}
