using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Threading.Tasks;

public class GoogleStorageHelper
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleStorageHelper(string bucketName)
    {
        _storageClient = StorageClient.Create();
        _bucketName = bucketName;
    }

    /// <summary>
    /// Sube un archivo a Google Cloud Storage desde un MemoryStream.
    /// </summary>
    /// <param name="memoryStream">Stream de memoria que contiene los datos del archivo.</param>
    /// <param name="objectName">Nombre con el que se almacenará en GCS.</param>
    public async Task UploadFileAsync(MemoryStream memoryStream, string objectName)
    {
        memoryStream.Position = 0; // Asegurar que el stream está al inicio
        await _storageClient.UploadObjectAsync(_bucketName, objectName, null, memoryStream);
        Console.WriteLine($"Archivo {objectName} subido exitosamente a {_bucketName}.");
    }

    /// <summary>
    /// Descarga un archivo desde Google Cloud Storage y lo retorna como MemoryStream.
    /// </summary>
    /// <param name="objectName">Nombre del archivo en GCS.</param>
    /// <returns>MemoryStream con los datos del archivo.</returns>
    public async Task<MemoryStream> DownloadFileAsync(string objectName)
    {
        var memoryStream = new MemoryStream();
        await _storageClient.DownloadObjectAsync(_bucketName, objectName, memoryStream);
        memoryStream.Position = 0; // Asegurar que el stream está al inicio
        Console.WriteLine($"Archivo {objectName} descargado exitosamente en memoria.");
        return memoryStream;
    }
}
