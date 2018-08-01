const path = require('path')
const webpack = require('webpack')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin')
const merge = require('webpack-merge')

const bundleOutputDir = './wwwroot/dist'

module.exports = (env) => {
    console.log('Env: ' + env)
    console.log('Building for \x1b[33m%s\x1b[0m', process.env.NODE_ENV || 'development')

    const isDevBuild = !(process.env.NODE_ENV && process.env.NODE_ENV === 'production')
    const extractCSS = new ExtractTextPlugin('site.css')

    // Configuration in common to both client-side and server-side bundles

    const sharedConfig = () => ({
        stats: { modules: false },
        resolve: { extensions: ['.js', '.vue'], },
        output: {
            filename: '[name].js',
            publicPath: 'dist/', // Webpack dev middleware, if enabled, handles requests for this URL prefix
        },

        module: {
            rules: [
                { test: /\.vue?$/, include: /ClientApp/, use: 'vue-loader?silent=true' },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
            ]
        },
        //plugins: [new CheckerPlugin()]
    });



    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = './wwwroot/dist';
    const clientBundleConfig = merge(sharedConfig(), {
        entry: { 'main-client': './ClientApp/boot-app.js' },

        module: {
            rules: [
                { test: /\.vue$/, include: /ClientApp/, use: 'vue-loader' },
                { test: /\.js$/, include: /ClientApp/, use: 'babel-loader' },
                { test: /\.css$/, use: ExtractTextPlugin.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) }
                // { test: /\.css$/, use: isDevBuild ? ['style-loader', 'css-loader'] : ExtractTextPlugin.extract({ use: 'css-loader' }) },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
            ]
        },
        output: { path: path.join(__dirname, clientBundleOutputDir) },
        plugins: [
            new ExtractTextPlugin('site.css'),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
            // Plugins that apply in production builds only
            new webpack.optimize.UglifyJsPlugin()
        ])
    });
    
    // Configuration for server-side (prerendering) bundle suitable for running in Node
    const serverBundleConfig = merge(sharedConfig(), {
        resolve: { mainFields: ['main'] },
        entry: { 'main-server': './ClientApp/boot-server.js' },
        alias: isDevBuild ? {
                'vue$': 'vue/dist/vue',
                'components': path.resolve(__dirname, './ClientApp/components'),
                //'views': path.resolve(__dirname, './ClientApp/views'),
                //'utils': path.resolve(__dirname, './ClientApp/utils'),
                //'api': path.resolve(__dirname, './ClientApp/store/api')
            } : {
                'components': path.resolve(__dirname, './ClientApp/components'),
                //'views': path.resolve(__dirname, './ClientApp/views'),
                //'utils': path.resolve(__dirname, './ClientApp/utils'),
                //'api': path.resolve(__dirname, './ClientApp/store/api')
            },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./ClientApp/dist/vendor-manifest.json'),
                sourceType: 'commonjs2',
                name: './vendor'
            })
        ],
        output: {
            libraryTarget: 'commonjs',
            path: path.join(__dirname, './ClientApp/dist')
        },
        target: 'node',
        devtool: 'inline-source-map'
    });



    return [clientBundleConfig];

    //return [{
    //    stats: { modules: false },
    //    entry: { 'main': './ClientApp/boot-app.js' },
    //    resolve: {
    //        extensions: ['.js', '.vue'],
    //        alias: isDevBuild ? {
    //            'vue$': 'vue/dist/vue',
    //            'components': path.resolve(__dirname, './ClientApp/components'),
    //            //'views': path.resolve(__dirname, './ClientApp/views'),
    //            //'utils': path.resolve(__dirname, './ClientApp/utils'),
    //            //'api': path.resolve(__dirname, './ClientApp/store/api')
    //        } : {
    //            'components': path.resolve(__dirname, './ClientApp/components'),
    //            //'views': path.resolve(__dirname, './ClientApp/views'),
    //            //'utils': path.resolve(__dirname, './ClientApp/utils'),
    //            //'api': path.resolve(__dirname, './ClientApp/store/api')
    //        }
    //    },
    //    output: {
    //        path: path.join(__dirname, bundleOutputDir),
    //        filename: '[name].js',
    //        publicPath: '/dist/'
    //    },
    //    module: {
    //        rules: [
    //            { test: /\.vue$/, include: /ClientApp/, use: 'vue-loader' },
    //            { test: /\.js$/, include: /ClientApp/, use: 'babel-loader' },
    //            { test: /\.css$/, use: isDevBuild ? ['style-loader', 'css-loader'] : ExtractTextPlugin.extract({ use: 'css-loader' }) },
    //            { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
    //        ]
    //    },
    //    plugins: [
    //        //new webpack.DllReferencePlugin({
    //        //    context: __dirname,
    //        //    manifest: require('./wwwroot/dist/vendor-manifest.json')
    //        //})
    //    ].concat(isDevBuild ? [
    //        // Plugins that apply in development builds only
    //        new webpack.SourceMapDevToolPlugin({
    //            filename: '[file].map', // Remove this line if you prefer inline source maps
    //            moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
    //        })
    //    ] : [
    //        // Plugins that apply in production builds only
    //        new webpack.optimize.UglifyJsPlugin(),
    //        extractCSS,
    //        // Compress extracted CSS.
    //        new OptimizeCSSPlugin({
    //            cssProcessorOptions: {
    //                safe: true
    //            }
    //        })
    //    ])
    //}]
}