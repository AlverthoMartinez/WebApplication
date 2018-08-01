const path = require('path')
const webpack = require('webpack')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin')
const merge = require('webpack-merge')

module.exports = () => {
    console.log('Building vendor files for \x1b[33m%s\x1b[0m', process.env.NODE_ENV || 'development')

    const isDevBuild = !(process.env.NODE_ENV && process.env.NODE_ENV === 'production')

    //const sharedConfig = {
    //    stats: { modules: false },
    //    resolve: { extensions: ['.js'] },
    //    module: {
    //        rules: [
    //            { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' }
    //        ]
    //    },
    //    entry: {
    //        vendor: [
    //            'event-source-polyfill',
    //            'vue',
    //            'vuex',
    //            'axios',
    //            'vue-router',
    //        ],
    //    },
    //    output: {
    //        publicPath: 'dist/',
    //        filename: '[name].js',
    //        library: '[name]_[hash]',
    //    },
    //    plugins: [
    //        new webpack.NormalModuleReplacementPlugin(/\/iconv-loader$/, require.resolve('node-noop')), // Workaround for https://github.com/andris9/encoding/issues/16
    //        new webpack.DefinePlugin({
    //            'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
    //        })
    //    ]
    //};

    //const clientBundleConfig = merge(sharedConfig, {
    //    output: { path: path.join(__dirname, 'wwwroot', 'dist') },
    //    module: {
    //        rules: [
    //            { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) }
    //        ]
    //    },
    //    plugins: [
    //        extractCSS,
    //        new webpack.DllPlugin({
    //            path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
    //            name: '[name]_[hash]'
    //        })
    //    ].concat(isDevBuild ? [] : [
    //        new webpack.optimize.UglifyJsPlugin()
    //    ])
    //});

    //const serverBundleConfig = merge(sharedConfig, {
    //    target: 'node',
    //    resolve: { mainFields: ['main'] },
    //    output: {
    //        path: path.join(__dirname, 'ClientApp', 'dist'),
    //        libraryTarget: 'commonjs2',
    //    },
    //    module: {
    //        rules: [{ test: /\.css(\?|$)/, use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }]
    //    },
    //    entry: { vendor: ['aspnet-prerendering', 'react-dom/server'] },
    //    plugins: [
    //        new webpack.DllPlugin({
    //            path: path.join(__dirname, 'ClientApp', 'dist', '[name]-manifest.json'),
    //            name: '[name]_[hash]'
    //        })
    //    ]
    //});

    // return [clientBundleConfig];
    const extractCSS = new ExtractTextPlugin('vendor.css')

    return [{
        stats: { modules: false },
        resolve: {
            extensions: ['.js'],
        },
        module: {
            rules: [
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' },
                { test: /\.css(\?|$)/, use: extractCSS.extract(['css-loader']) },
            ]
        },
        entry: {
            vendor: [
                'event-source-polyfill',
                'vue',
                'vuex',
                'axios',
                'vue-router',
            ],
        },
        output: {
            path: path.join(__dirname, 'wwwroot', 'dist'),
            publicPath: '/dist/',
            filename: '[name].js',
            library: '[name]_[hash]',
        },
        plugins: [
            extractCSS,
            // Compress extracted CSS.
            new OptimizeCSSPlugin({
                cssProcessorOptions: {
                    safe: true,
                }
            }),
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]',
            }),
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
            })
        ].concat(isDevBuild ? [] : [
            new webpack.optimize.UglifyJsPlugin()
        ])
    }]
}