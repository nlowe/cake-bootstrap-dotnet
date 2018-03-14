# Cake Bootstrapper for dotnet core projects

Bootstrap cake for dotnet core projects without needing to install mono. Options (environment
variables):

* `TOOLS_DIR`: The path to install cake tools to. `./tools` by default
* `CAKE_VERSION`: The version of cake to install. `0.26.1` by default. To upgrade cake, delete your `TOOLS_DIR` and change this variable.
* `CAKE_NETCOREAPP_VERSION`: The `netcoreapp` version to use for the tools dummy project. `2.0` by default. Must be compatible with [`Cake.CoreCLR`](https://www.nuget.org/packages/Cake.CoreCLR/)

All other options are present as with the standard
[bootstrap](https://github.com/cake-build/example) scripts.