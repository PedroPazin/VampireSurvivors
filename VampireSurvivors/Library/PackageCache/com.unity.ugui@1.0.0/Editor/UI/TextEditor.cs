e008b58bb968729775fa5') in 0.005991 seconds
Refreshing native plugins compatible for Editor in 1.86 ms, found 4 plugins.
Native extension for WindowsStandalone target not found
Mono: successfully reloaded assembly
- Finished resetting the current domain, in  1.384 seconds
Domain Reload Profiling: 2172ms
	BeginReloadAssembly (148ms)
		ExecutionOrderSort (0ms)
		DisableScriptedObjects (5ms)
		BackupInstance (0ms)
		ReleaseScriptingObjects (0ms)
		CreateAndSetChildDomain (26ms)
	RebuildCommonClasses (34ms)
	RebuildNativeTypeToScriptingClass (10ms)
	initialDomainReloadingComplete (33ms)
	LoadAllAssembliesAndSetupDomain (563ms)
		LoadAssemblies (475ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (166ms)
			TypeCache.Refresh (130ms)
				TypeCache.ScanAssembly (117ms)
			ScanForSourceGeneratedMonoScriptInfo (19ms)
			ResolveRequiredComponents (16ms)
	FinalizeReload (1384ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (1261ms)
			LogAssemblyErro