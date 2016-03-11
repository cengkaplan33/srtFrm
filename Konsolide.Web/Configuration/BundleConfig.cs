using System.Web;
using System.Web.Optimization;

namespace Konsolide.Web
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Özelleştirme            
        }

        public class CssRewriteUrlTransformWrapper : IItemTransform
        {
            public string Process(string includedVirtualPath, string input)
            {
                return new CssRewriteUrlTransform().Process(
                    "~" + VirtualPathUtility.ToAbsolute(includedVirtualPath),
                    input);
            }
        }
    }

}
