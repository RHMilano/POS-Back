using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository.Security;

namespace Milano.BackEnd.UniTest
{
    [TestClass]
    public class SecurityBusinessTest
    {
        [TestMethod]
        public void LogOut()
        {
            OperationResponse response = new OperationResponse();
            TokenDto token = new TokenDto(3215, 3);
            var securityRepository = new SecurityRepository();
            response=  securityRepository.Logout(1234, token.CodeStore, token.CodeBox);

            Assert.IsTrue(response.CodeNumber == "");
        }

    }
}
