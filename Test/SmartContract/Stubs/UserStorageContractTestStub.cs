using EthereumLibrary.Helper;

namespace Test.SmartContract.Stubs
{
    public static class UserStorageContractTestStub
    {
        public static byte[] AdminLogin => CastHelper.StringToBytes32("admin");
        public static byte[] AdminPassword => CastHelper.StringToBytes32("admin123");

        public static byte[] UserLogin1 => CastHelper.StringToBytes32("john");
        public static byte[] UserLogin2 => CastHelper.StringToBytes32("jack");

        public static byte[] UserPassword1 => CastHelper.StringToBytes32("qwerty123");
        public static byte[] UserPassword2 => CastHelper.StringToBytes32("asdfgh321");
        public static byte[] UserPassword3 => CastHelper.StringToBytes32("123456789");

        public static byte[][] FirstName1 => CastHelper.StringToBytes32ArrayOf(2, "John");
        public static byte[][] FirstName2 => CastHelper.StringToBytes32ArrayOf(2, "Jack");

        public static byte[][] LastName1 => CastHelper.StringToBytes32ArrayOf(2, "Doe");
        public static byte[][] LastName2 => CastHelper.StringToBytes32ArrayOf(2, "Smith");

        public static byte[][] Info1 => CastHelper.StringToBytes32ArrayOf(6, "Cool test guy");
        public static byte[][] Info2 => CastHelper.StringToBytes32ArrayOf(6, "The coolest test guy!");
    }
}