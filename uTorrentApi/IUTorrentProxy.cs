
namespace UTorrentAPI.Service
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IUTorrentProxy // TODO: Should be internal
    {
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?list=1"
            )]
        void GetAllTorrentsAndLabels();
    }
}
