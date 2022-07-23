
<div id="top"></div>

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
	<h3 align="center">ExplicitLayers for .Net</h3> 
</a>

  <p align="center">
    ExplicitLayers enables you to put in cross-layer dependency checks in your project. 
    <br />
       ·
    <a href="https://github.com/thestonehead/ExplicitLayers/issues">Report Bug</a>
    ·
    <a href="https://github.com/thestonehead/ExplicitLayers/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Are you tired of having discussions with your team about how to structure your project structure? Are you sick of having layers conflated with projects just to stop "bad developers" from referencing wrong things? This is exactly what you need.
This nuget will add two things to your project:

 - A Layer attribute for specifying to which layer a type (class, struct, interface) belongs to
 - Analyzers which will warn you about forbidden references between layers

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

The nuget is built for .Net Standard 2.0 so it should work for both .Net Framework and .Net (Core) projects.

### Installation

1. Add a nuget to your project
2. Add configuration to your .editorconfig file

```
dotnet_diagnostic.ExplicitLayers.comma_separated_layer_names = Domain,Infrastructure,Web
dotnet_diagnostic.ExplicitLayers.Infrastructure.comma_separated_allowed_dependencies = Domain
dotnet_diagnostic.ExplicitLayers.Web.comma_separated_allowed_dependencies = Domain,Infrastructure
dotnet_diagnostic.ExplicitLayers.Web.comma_separated_regex_paths = .*Web.*
```

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Add `[Layer("<layer-name>")]` to your classes to specify to which layer they belong.

Add all layer names to .editorconfig config as a comma separated list `dotnet_diagnostic.ExplicitLayers.comma_separated_layer_names` .

Add rules for every layer to .editorconfig as a comma separated list `dotnet_diagnostic.ExplicitLayers.<layer-name>.comma_separated_allowed_dependencies` .

Optionally add rules to .editorconfig to specify all types on a certain path belonging to a certain layer as comma separated list of regex patterns `dotnet_diagnostic.ExplicitLayers.<layer-name>.comma_separated_regex_paths`.

Example of path resolution
![enter image description here](https://github.com/thestonehead/ExplicitLayers/blob/master/docs/ShowPathResolution.png?raw=true)

Example of warning for forbidden referencing
![enter image description here](https://github.com/thestonehead/ExplicitLayers/blob/master/docs/ShowWarningBetweenClasses.png?raw=true)

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- CONTRIBUTING -->
## Contributing

You're welcome to contribute if you think it needs something more.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the GPL License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

TheStonehead

Project Link: [https://github.com/thestonehead/ExplicitLayers](https://github.com/thestonehead/ExplicitLayers)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
