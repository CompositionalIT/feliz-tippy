import React from "react";
import path from "path";

/**
 * Plugins specific to this layout
 */
const remarkPlugins =
    [
        {
            Resolve: "remark-code-import"
        },
        {
            Resolve: "gatsby-remark-vscode",
            Property: "remarkPlugin",
            Options: {
                "theme": "Atom One Light",
                "extensions": [
                    "vscode-theme-onelight"
                ]
            }
        }
    ]

/**
 * Include live-reload.js script when in watch mode
 */
const LiveReloadScript = ({ rendererContext }) => {
    if (rendererContext.Config.IsWatch) {
        const scriptSrc =
            rendererContext.Config.SiteMetadata.BaseUrl + "resources/nacara/scripts/live-reload.js";

        return <script async src={scriptSrc}></script>
    } else {
        return null;
    }
}

/**
 * Generate a minimal Navbar
 *
 * Note: This navbar doesn't generate any navbar items, coming from the navbar config
 */
const Navbar = ({ rendererContext, github_link }) => {
    return (
        <div className="navbar is-primary is-spaced">
            <div className="container">
                <div className="navbar-brand">
                    <div className="navbar-item">
                        <img
                            style={{ height: "50px", maxHeight: "50px" }}
                            src={rendererContext.Config.SiteMetadata.BaseUrl + "public/img/cit-logo.png"}
                            alt="Nacara" />
                        <div className="subtitle is-2 ml-2 has-text-white">
                            Compositional IT
                        </div>
                    </div>
                </div>
                <div className="navbar-menu">
                    <div className="navbar-end">
                        <div className="navbar-item">
                            <a className="button is-primary" href={github_link}>
                                <span className="icon is-large">
                                    <i className="fab fa-2x fa-github"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

/**
 * Specilized container which contains a Title and some content in it
 *
 * This container is to be used to create specialized section in the page
 */
const CITContainer = ({title, children}) => {
    return (
        <div className="mt-5">
            <h2 className="subtitle pb-2 has-border-bottom">{title}</h2>
            <div className="content">
                {children}
            </div>
        </div>
    )
}

/**
 * Generates a link for the introduction section
 */
const IntroductionLink = ({url, text}) => {
    return (
        <li>
            <a className="has-text-weight-bold is-underlined" href={url}>{text}</a>
        </li>
    )
}

/**
 * Generate the introduction of the page
 */
const Introduction = ({title, introduction, npm_link, nuget_link}) => {

    return (
        <CITContainer title={title}>
            <p>{introduction}</p>
            <ul>
                <IntroductionLink href={npm_link} text="npm" />
                <IntroductionLink href={nuget_link} text="nuget" />
            </ul>
        </CITContainer>
    )
}

/**
 * Generate the demo section
 *
 * This create a div where demo can attach themselves
 */
const DemoContainer = () => {
    return (
        <CITContainer title="Demo">
            <div id="demo-container"></div>
        </CITContainer>
    )
}

/**
 * Generate the installation section
 *
 * The content of this section comes from the 'installation_instruction' front-matter attributes
 * The 'installation_instruction' is being process as markdown
 */
const Installation = ({instruction}) => {


    return (
        <CITContainer title="Installation">
            <div dangerouslySetInnerHTML={{ __html: instruction }} />
        </CITContainer>
    )
}

/**
 * Generate the sample code section
 *
 * The content of this section is generated from the 'sample_file' front-matter attribute
 * The content of the file located at 'sample_file' is being include a code blocks
 * and process as markdown
 *
 * This allows for using code coming from the actual example files,
 * which means we can check that the code actually compiles before publishing
 * the documentation
 */
const SampleCode = ({htmlContent}) => {
    return (
        <CITContainer title="Sample Code">
            <div dangerouslySetInnerHTML={{ __html: htmlContent }} />
        </CITContainer>
    )
}

const PageContainer = ({ rendererContext, pageContext, attributes, pageContent }) => {
    let titleStr = rendererContext.Config.SiteMetadata.Title;

    if (pageContext.Title) {
        titleStr = rendererContext.Config.SiteMetadata.Title + " · " + pageContext.Title
    }

    const styleHref = rendererContext.Config.SiteMetadata.BaseUrl + "style.css"

    return (
        <html>
            <meta lang="en" />

            <head>
                <title>{titleStr}</title>
                <meta httpEquiv="Content-Type" content="text/html; charset=utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="shortcut icon" type="image/png" href="public/img/favicon-16x16.png" sizes="16x16"></link>
                <link rel="shortcut icon" type="image/png" href="public/img/favicon-32x32.png" sizes="32x32"></link>
                <link rel="stylesheet" href={styleHref} />
            </head>

            <body>
                <Navbar
                    rendererContext={rendererContext}
                    github_link={attributes.github_link} />

                <div className="container section">
                    <Introduction
                        title={pageContext.Title}
                        introduction={attributes.introduction}
                        npm_link={attributes.npm_link}
                        nuget_link={attributes.nuget_link} />

                    <DemoContainer />
                    <Installation instruction={attributes.installation_instruction} />
                    <SampleCode htmlContent={attributes.sample_html} />

                    <div dangerouslySetInnerHTML={{ __html: pageContent }} />
                </div>

                <LiveReloadScript rendererContext={rendererContext} />
            </body>

        </html>
    )
}

const extractAttributes = (attributes) => {
    const extractAttribute = (attribute) => {
        const res = attributes[attribute]

        if (res) {
            return res;
        } else {
            throw `Missing '${attributes}' attribute`;
        }
    }

    return {
        installation_instruction : extractAttribute("installation_instruction"),
        npm_link : extractAttribute("npm_link"),
        nuget_link : extractAttribute("nuget_link"),
        introduction : extractAttribute("introduction"),
        sample_file : extractAttribute("sample_file"),
        github_link : extractAttribute("github_link")
    }
}

const preProcessMarkdown = async (rendererContext, pageContext, attributes) => {
    attributes.installation_instruction =
        await rendererContext.MarkdownToHtml(
            attributes.installation_instruction,
            pageContext.RelativePath,
            remarkPlugins
        );

    // We are going to use the extension file for the syntax highlighting
    let sampleExtension =
        path.extname(attributes.sample_file).substr(1);

    // If a line number has been specified, we need to remove it
    // to get the file extension
    // Example:
    //  ./../demo/src/Components.fs#L9-
    //      => fs#L9
    //      => fs
    if (sampleExtension.indexOf("#") !== -1) {
        sampleExtension = sampleExtension.substr(0, sampleExtension.indexOf("#"));
    }

    // Generate a code blocks which will import the sample file content
    // before applying syntax highlighting to it
    // IMPORTANT: This is controlled by the plugins order
    const sampleMarkdown =
        `
\`\`\`${sampleExtension} file=${attributes.sample_file}
\`\`\`
        `

    attributes.sample_html =
        await rendererContext.MarkdownToHtml(
            sampleMarkdown,
            pageContext.RelativePath,
            remarkPlugins
        );
}

const render = async (rendererContext, pageContext) => {
    const attributes = extractAttributes(pageContext.Attributes);

    await preProcessMarkdown(rendererContext, pageContext, attributes);

    const pageContent =
        await rendererContext.MarkdownToHtml(
            pageContext.Content,
            pageContext.RelativePath,
            remarkPlugins
        );

    return <PageContainer
        rendererContext={rendererContext}
        pageContext={pageContext}
        pageContent={pageContent}
        attributes={attributes} />;
}

export default {
    Renderers: [
        {
            Name: "simple-doc",
            Func: render
        }
    ],
    Dependencies: []
}
