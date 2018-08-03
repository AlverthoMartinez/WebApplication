'use strict'

const path = require('path')
const utils = require('./utils')
const webpack = require('webpack')
const config = require('../config')
const merge = require('webpack-merge')
const baseWebpackConfig = require('./webpack.base.conf')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const loadMinified = require('./load-minified')
const FriendlyErrorsPlugin = require('friendly-errors-webpack-plugin')

const env = process.env.NODE_ENV === 'testing'
  ? require('../config/test.env')
  : config.build.env

const webpackConfig = merge(baseWebpackConfig, {
  module: {
    rules: utils.styleLoaders({
      sourceMap: config.build.productionSourceMap,
    }),
  },
  devtool: config.build.productionSourceMap ? '#source-map' : false,
  output: {
    path: config.build.assetsRoot,
    filename: utils.assetsPath('js/[name].js'),
    chunkFilename: utils.assetsPath('js/[id].js'),
    publicPath: '/dist/',
  },
  plugins: [

    new webpack.DefinePlugin({
      'process.env': config.dev.env,
    }),
    // Plugins that apply in development builds only
    new webpack.SourceMapDevToolPlugin({
      // Remove this line if you prefer inline source maps
      filename: '[file].map',
      // Point sourcemap entries to the original file locations on disk
      moduleFilenameTemplate: path.relative(config.build.assetsRoot, '[resourcePath]'),
    }),
    // generate dist index.html with correct asset hash for caching.
    // you can customize output by editing /index.html
    // see https://github.com/ampedandwired/html-webpack-plugin
    new HtmlWebpackPlugin({
      filename: process.env.NODE_ENV === 'testing'
        ? 'index.html'
        : config.build.index,
      template: 'index.html',
      // necessary to consistently work with multiple chunks via CommonsChunkPlugin
      chunksSortMode: 'dependency',
      serviceWorkerLoader: `<script>${loadMinified(path.join(__dirname,
        './service-worker-prod.js'))}</script>`,
    }),
    new FriendlyErrorsPlugin(),
  ],
})

module.exports = webpackConfig
