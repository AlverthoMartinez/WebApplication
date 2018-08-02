import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
// import { sync } from 'vuex-router-sync'
import App from './components/app'

Vue.prototype.$http = axios

// sync(store, router)

const app = new Vue(Object.assign({
  // store,
  router,
}, App))

export {
  app,
  router,
  // store
}
