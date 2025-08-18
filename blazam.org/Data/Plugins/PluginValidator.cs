using System.Runtime.Loader;
using blazam.org.Data.Plugins.Models;
using BLAZAM.Plugins;
using Microsoft.AspNetCore.Components.Forms;

namespace blazam.org.Data.Plugins
{
    public class PluginValidator
    {
        public static async Task<BlazamPlugin?> ValidateUploadAsync(IBrowserFile file)
        {
            // Step 1: Check if file is a valid zip
            if (!file.ContentType.Equals("application/zip", StringComparison.OrdinalIgnoreCase) &&
                !file.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("Uploaded file is not a valid zip archive.");
            }

            var tempZipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");
            var tempExtractDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                // Step 2: Save to temp file
                using (var stream = File.Create(tempZipPath))
                using (var fileStream = file.OpenReadStream())
                {
                    await fileStream.CopyToAsync(stream);
                }

                // Step 3: Unzip to temp directory
                Directory.CreateDirectory(tempExtractDir);
                System.IO.Compression.ZipFile.ExtractToDirectory(tempZipPath, tempExtractDir);

                // Step 4: Find all DLLs
                var dllFiles = Directory.GetFiles(tempExtractDir, "*.dll", SearchOption.AllDirectories);

                // Step 5: Load assemblies and find IPluginBase implementation

                foreach (var dll in dllFiles)
                {
                    AssemblyLoadContext? loadContext = null;
                    try
                    {
                        // Create a new AssemblyLoadContext for isolation and unloading
                        loadContext = new AssemblyLoadContext(Guid.NewGuid().ToString(), isCollectible: true);

                        var assembly = loadContext.LoadFromAssemblyPath(dll);
                        var pluginType = assembly.GetTypes()
                            .FirstOrDefault(t => typeof(IPluginBase).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

                        if (pluginType != null)
                        {
                            var instance = Activator.CreateInstance(pluginType) as IPluginBase;
                            if (instance != null)
                            {
                                // Unload the context after instance creation
                                var plugin = new BlazamPlugin
                                {
                                    Name = instance.Name,
                                    Version = instance.Version.ToString(),
                                    Author = instance.Author
                                };

                                instance = null;
                                loadContext.Unload();
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                return plugin;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ignore load errors and continue
                    }
                    finally
                    {
                        // Ensure context is unloaded if not already
                        if (loadContext != null)
                        {
                            loadContext.Unload();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }


                throw new InvalidOperationException("No valid IPluginBase implementation found in uploaded zip.");
            }
            catch (Exception ex)
            {
                // Ignore load errors and continue
                return null;
            }
            finally
            {
                Task.Run(() =>
                {
                    Task.Delay(30000).Wait();
                    // Cleanup temp files, ignore errors
                    try { if (File.Exists(tempZipPath)) File.Delete(tempZipPath); } catch { }
                    try { if (Directory.Exists(tempExtractDir)) Directory.Delete(tempExtractDir, true); } catch { }
                });

            }
        }
    }
}
