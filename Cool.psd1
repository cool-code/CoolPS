#
# Module manifest for module 'Cool'
#

@{
    # Script module or binary module file associated with this manifest.
    RootModule        = 'Cool.psm1'

    # Version number of this module.
    ModuleVersion     = '0.1.3'

    # Supported PSEditions
    # CompatiblePSEditions = @()

    # ID used to uniquely identify this module
    GUID              = '7a2f8c5b-1234-4a5d-9e6b-abcdef123456'

    # Author of this module
    Author            = 'Ma Bingyao'

    # Company or vendor of this module
    CompanyName       = 'coolcode.org'

    # Copyright statement for this module
    Copyright         = '(c) 2026 Cool-Code. All rights reserved.'

    # Description of the functionality provided by this module
    Description       = 'A PowerShell module that provides Linux-style color and icon support for the Windows terminal, enhancing the visual experience of command-line interactions.'

    # Minimum version of the Windows PowerShell engine required by this module
    PowerShellVersion = '5.1'

    # Name of the Windows PowerShell host required by this module
    # PowerShellHostName = ''

    # Minimum version of the Windows PowerShell host required by this module
    # PowerShellHostVersion = ''

    # Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # DotNetFrameworkVersion = ''

    # Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # CLRVersion = ''

    # Processor architecture (None, X86, Amd64) required by this module
    # ProcessorArchitecture = ''

    # Modules that must be imported into the global environment prior to importing this module
    # RequiredModules = @()

    # Assemblies that must be loaded prior to importing this module
    # RequiredAssemblies = @()

    # Script files (.ps1) that are run in the caller's environment prior to importing this module.
    # ScriptsToProcess = @()

    # Type files (.ps1xml) to be loaded when importing this module
    # TypesToProcess = @()

    # Format files (.ps1xml) to be loaded when importing this module
    # FormatsToProcess = @()

    # Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
    NestedModules     = @()

    # Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
    FunctionsToExport = @(
        'Format-CoolName'
        'Format-CoolSize'
        'Get-VisualWidth'
        'Format-VisualWidthString'
        '*'
    )

    # Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
    CmdletsToExport   = @()

    # Variables to export from this module
    VariablesToExport = ''

    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    AliasesToExport   = @('cd', 'ls')

    # DSC resources to export from this module
    # DscResourcesToExport = @()

    # List of all modules packaged with this module
    # ModuleList        = @()

    # List of all files packaged with this module
    FileList          = @(
        'Cool.psd1'
        'Cool.psm1'
        'PSReadLine.ps1'
        'LazyLoad.ps1'
        'Private/Localization.ps1'
        'Private/ColorAndIcon.ps1'
        'Private/VisualWidth.ps1'
        'LICENSE'
        'Functions/cd.ps1'
        'Functions/cool.ps1'
        'Functions/ls.ps1'
        'Data/LS_COLORS'
        'Data/LS_ICONS'
    )

    # Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
    PrivateData       = @{

        PSData           = @{

            # Tags applied to this module. These help with module discovery in online galleries.
            Tags       = @('Cool', 'Utility', 'Linux-Style', 'LS_COLORS', 'LS_ICONS', 'CLI')

            # A URL to the license for this module.
            LicenseUri = 'https://github.com/cool-code/CoolPS/blob/main/LICENSE'

            # A URL to the main website for this project.
            ProjectUri = 'https://github.com/cool-code/CoolPS'

            # A URL to an icon representing this module.
            IconUri    = 'https://avatars.githubusercontent.com/u/23434088'

            # ReleaseNotes of this module
            # ReleaseNotes = ''

        } # End of PSData hashtable

        CommandsToExport = @{
            cool = @('cool')
            cd   = @(
                'Set-CurrentDirectory'
                '~'
                '..'
                '...'
                '....'
                '.....'
                '......'
                '.......'
                '........'
                '.........'
                '..........'
                '...........'
                '............'
                '.............'
                '..............'
                '...............'
                '................'
                '.................'
                '..................'
                '...................'
                '....................'
                '.....................'
                '/'
                '//'
                '///'
                '////'
                '/////'
                '//////'
                '///////'
                '////////'
                '/////////'
                '//////////'
                '///////////'
                '////////////'
                '/////////////'
                '//////////////'
                '///////////////'
                '////////////////'
                '/////////////////'
                '//////////////////'
                '///////////////////'
                '////////////////////'
                '\'
                '\\'
                '\\\'
                '\\\\'
                '\\\\\'
                '\\\\\\'
                '\\\\\\\'
                '\\\\\\\\'
                '\\\\\\\\\'
                '\\\\\\\\\\'
                '\\\\\\\\\\\'
                '\\\\\\\\\\\\'
                '\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\\\\\'
                '\\\\\\\\\\\\\\\\\\\\'               
            )
            ls   = @('l')
        }

    } # End of PrivateData hashtable

    # HelpInfo URI of this module
    # HelpInfoURI = ''

    # Default prefix for commands  from this module. Override the default prefix using Import-Module -Prefix.
    # DefaultCommandPrefix = ''

}