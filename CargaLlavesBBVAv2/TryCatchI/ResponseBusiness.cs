namespace CargaLlavesBBVAv2.TryCatchI
{
    public class ResponseBusiness<T>
    {
        /// <summary>
        /// Constructor por default
        /// </summary>
        public ResponseBusiness()
        {
            Result = new StatusRequest();
            Result.Status = true;
        }

        public StatusRequest Result { get; set; }


        public T Data { get; set; }


        public static implicit operator T(ResponseBusiness<T> value)
        {
            return value.Data;
        }

        public static implicit operator ResponseBusiness<T>(T value)
        {
            return new ResponseBusiness<T> { Data = value };
        }
    }
}
