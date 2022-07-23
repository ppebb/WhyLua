using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using WhyLua.Content;

namespace WhyLua {
	public class WhyLua : Mod {
		public static string WhyLuaPath => Path.Combine(Main.SavePath, "WhyLua");
		public static string Lua54Path => Path.Combine(WhyLuaPath, LuaLib());
		public static Lua _interpreter;
		private static IntPtr NativeLib;

		public override void Load() {
			Directory.CreateDirectory(WhyLuaPath);
			byte[] lua54Bytes = GetFileBytes(Path.Combine("lib", LuaLib()));
			File.WriteAllBytes(Lua54Path, lua54Bytes);
			NativeLibrary.SetDllImportResolver(typeof(KeraLua.Lua).Assembly, DllImportResolver);

			Lua _interpreter = new();
			_interpreter.LoadCLRPackage();
			_interpreter.State.Encoding = Encoding.UTF8;
		}
		
		public override void Unload()
		{
			NativeLibrary.Free(NativeLib);
		}

		private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) {
			if (libraryName == "lua54") {
				if(NativeLib != IntPtr.Zero)
				{
					return NativeLib;
				}
				NativeLib = NativeLibrary.Load(Lua54Path);
				return NativeLib;
			}

			return IntPtr.Zero;
		}

		private static string LuaLib() {
			if (OperatingSystem.IsWindows())
				return "lua54.dll";
			else if (OperatingSystem.IsMacOS())
				return "liblua54.dylib";
			else if (OperatingSystem.IsLinux())
				return "liblua54.so";
			else
				throw new NotImplementedException();
		}


		public static void LoadLua(Mod luaMod) {
			List<string> files = luaMod.GetFileNames();

			foreach (string file in files) {
				if (Path.GetExtension(file) == ".lua") {
					string luaFile = Encoding.UTF8.GetString(luaMod.GetFileBytes(file));

					_interpreter.DoString(luaFile);
					LuaFunction typeFunction = _interpreter["contentType"] as LuaFunction;
					string contentType = (string)typeFunction.Call()[0];

					switch(contentType) {
						case "modItem":
							luaMod.AddContent(new LuaItem(_interpreter, luaFile, file));
							break;
						// Add cases for other content types later
					}
				}
			}
		}
	}
}
