const path = require("path");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ReactRefreshWebpackPlugin = require("@pmmmwh/react-refresh-webpack-plugin");

function resolve(filePath) {
    return path.join(__dirname, filePath)
}

module.exports = (_env, options) => {

    var isDevelopment = options.mode === "development";

    return {
        // In development, bundle styles together with the code so they can also
        // trigger hot reloads. In production, put them in a separate CSS file.
        entry:
        {
            app: "./fableBuild/Main.js"
        },
        // Add a hash to the output file name in production
        // to prevent browser caching if code changes
        output: {
            path: resolve("./../docs/public/js"),
            filename: "demo.js"
        },
        devtool: isDevelopment ? 'eval-source-map' : false,
        plugins:
            [
                // In production, we only need the bundle file
                isDevelopment && new HtmlWebpackPlugin({
                    filename: "./index.html",
                    template: "./src/index.html"
                }),
                isDevelopment && new MiniCssExtractPlugin(),
                isDevelopment && new ReactRefreshWebpackPlugin()
            ].filter(Boolean),
        // Configuration for webpack-dev-server
        devServer: {
            port: 8080,
            hot: true
        },
        module: {
            rules: [
                {
                    test: /\.[jt]sx?$/,
                    exclude: /node_modules/,
                    use: [
                        {
                            loader: require.resolve('babel-loader'),
                            options: {
                                plugins: [
                                    isDevelopment && require.resolve('react-refresh/babel')
                                ].filter(Boolean)
                            },
                        },
                    ],
                },
                {
                    test: /\.(sass|scss|css)$/,
                    use: [
                        isDevelopment && MiniCssExtractPlugin.loader,
                        'css-loader',
                        'sass-loader',
                    ].filter(Boolean),
                },
                {
                    test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)$/,
                    use: ["file-loader"]
                }
            ]
        }
    }
}
