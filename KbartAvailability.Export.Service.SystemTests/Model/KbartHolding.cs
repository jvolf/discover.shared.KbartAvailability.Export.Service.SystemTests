using System;
using System.Collections.Generic;
using System.Text;

namespace KbartAvailability.Export.Service.SystemTests.Model
{
    public class KbartHolding
    {

        private const string PERPETUAL_LICENSETYPE = "Perpetual";
        private const string Dda_LICENSETYPE = "Dda";
        private const string ABOOK_CONTENTCLASS = "ABOOK";
        private const string EBOOK_CONTENTCLASS = "EBOOK";
        private const string PERPETUAL_ABOOK_COLLECTIONID = "nlabk";
        private const string PERPETUAL_EBOOK_COLLECTIONID = "nlebk";
        private const string DDA_ABOOK_COLLECTIONID = "nlabkpda";
        private const string DDA_EBOOK_COLLECTIONID = "nlebkpda";


        public int ProductCode { get; set; }
        public string CollectionId { get; set; }

        public KbartHolding(int productCode, string contentClass = null, string licenseType = null)
        {
            ProductCode = productCode;
            CollectionId = string.IsNullOrEmpty(contentClass) && string.IsNullOrEmpty(licenseType) ? string.Empty : CreateCollectionId(contentClass, licenseType);
        }


        private string CreateCollectionId(string contentClass, string licenseType)
        {
            if (licenseType == PERPETUAL_LICENSETYPE)
            {
                if (contentClass == ABOOK_CONTENTCLASS)
                    return PERPETUAL_ABOOK_COLLECTIONID;
                if (contentClass == EBOOK_CONTENTCLASS)
                    return PERPETUAL_EBOOK_COLLECTIONID;
            }
            else if (licenseType == Dda_LICENSETYPE)
            {
                if (contentClass == ABOOK_CONTENTCLASS)
                    return DDA_ABOOK_COLLECTIONID;
                if (contentClass == EBOOK_CONTENTCLASS)
                    return DDA_EBOOK_COLLECTIONID;
            }

            return string.Empty;
        }
    }
}
