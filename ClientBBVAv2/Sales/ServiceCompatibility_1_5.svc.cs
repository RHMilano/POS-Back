using BBVALogic.DTOCompatibility_1_5;
using BBVALogic.Retail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace ClientBBVAv2.Sales
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceCompatibility_1_5 : IServiceCompatibility_1_5
    {
        public PayVisaMasterCardResponse PayVisaMasterCard(PayVisaMasterCardRequest request)
        {
            ProcessSaleCompatibility_1_5 processSaleCompatibility_1_5 = new ProcessSaleCompatibility_1_5();
               PayVisaMasterCardResponse response = processSaleCompatibility_1_5.TryReadCard(request);
            return response;
        }
    }
}
