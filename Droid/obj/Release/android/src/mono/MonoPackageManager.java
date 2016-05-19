package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	public static void LoadApplication (Context context, ApplicationInfo runtimePackage, String[] apks)
	{
		synchronized (lock) {
			if (!initialized) {
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = getNativeLibraryPath (context);
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						getNativeLibraryPath (runtimePackage),
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				initialized = true;
			}
		}
	}

	static String getNativeLibraryPath (Context context)
	{
	    return getNativeLibraryPath (context.getApplicationInfo ());
	}

	static String getNativeLibraryPath (ApplicationInfo ainfo)
	{
		if (android.os.Build.VERSION.SDK_INT >= 9)
			return ainfo.nativeLibraryDir;
		return ainfo.dataDir + "/lib";
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		"GUC_Attendance.Droid.dll",
		"SQLite.Net.dll",
		"SQLite.Net.Platform.XamarinAndroid.dll",
		"SQLiteNetExtensions.dll",
		"System.Net.Http.Primitives.dll",
		"System.Net.Http.Extensions.dll",
		"Microsoft.WindowsAzure.Mobile.dll",
		"Microsoft.WindowsAzure.Mobile.Ext.dll",
		"Splat.dll",
		"Acr.Support.Android.dll",
		"Acr.UserDialogs.dll",
		"Acr.UserDialogs.Interface.dll",
		"AndHUD.dll",
		"Lotz.Xam.Messaging.Abstractions.dll",
		"Lotz.Xam.Messaging.dll",
		"BouncyCastle.dll",
		"MimeKit.dll",
		"MailKit.dll",
		"CsvHelper.dll",
		"Newtonsoft.Json.dll",
		"XLabs.Ioc.dll",
		"ExifLib.dll",
		"XLabs.Core.dll",
		"XLabs.Platform.Droid.dll",
		"XLabs.Platform.dll",
		"XLabs.Serialization.dll",
		"Plugin.Connectivity.dll",
		"Plugin.Connectivity.Abstractions.dll",
		"Sockets.Plugin.dll",
		"Sockets.Plugin.Abstractions.dll",
		"Xamarin.Android.Support.v4.dll",
		"Xamarin.Android.Support.Vector.Drawable.dll",
		"Xamarin.Android.Support.Animated.Vector.Drawable.dll",
		"Xamarin.Android.Support.v7.AppCompat.dll",
		"Xamarin.Android.Support.v7.RecyclerView.dll",
		"Xamarin.Android.Support.Design.dll",
		"Xamarin.Android.Support.v7.CardView.dll",
		"Xamarin.Android.Support.v7.MediaRouter.dll",
		"Xamarin.Forms.Platform.Android.dll",
		"FormsViewGroup.dll",
		"Xamarin.Forms.Core.dll",
		"Xamarin.Forms.Xaml.dll",
		"Xamarin.Forms.Platform.dll",
		"GUC_Attendance.dll",
		"System.Diagnostics.Tracing.dll",
		"System.Reflection.Emit.ILGeneration.dll",
		"System.Reflection.Emit.Lightweight.dll",
		"System.Reflection.Emit.dll",
		"System.ServiceModel.Security.dll",
		"System.Threading.Timer.dll",
		"System.ServiceModel.Internals.dll",
		"XLabs.Forms.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = null;
}
