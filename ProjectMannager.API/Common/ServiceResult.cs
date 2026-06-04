namespace ProjectMannager.API.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; } // Permitindo nulo para cenários de falha
        public bool HasChanges { get; private set; }

        private ServiceResult(bool success, string message, T? data, bool hasChanges)
        {
            Success = success;
            Message = message;
            Data = data;
            HasChanges = hasChanges;
        }

        public static ServiceResult<T> Ok(T data, string message = "Operação realizada com sucesso")
            => new ServiceResult<T>(true, message, data, true);

        public static ServiceResult<T> NoChanges(T data, string message = "Nenhuma alteração detectada")
            => new ServiceResult<T>(true, message, data, false);

        public static ServiceResult<T> Failure(string message)
            => new ServiceResult<T>(false, message, default, false);
    }

    // Sobrecarga não-genérica para métodos que não retornam dados (ex: Register, Delete)
    public class ServiceResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public bool HasChanges { get; private set; }

        private ServiceResult(bool success, string message, bool hasChanges)
        {
            Success = success;
            Message = message;
            HasChanges = hasChanges;
        }

        public static ServiceResult Ok(string message = "Operação realizada com sucesso")
            => new ServiceResult(true, message, true);

        public static ServiceResult NoChanges(string message = "Nenhuma alteração detectada")
            => new ServiceResult(true, message, false);

        public static ServiceResult Failure(string message)
            => new ServiceResult(false, message, false);
    }
}