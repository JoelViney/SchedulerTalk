
import Vue from 'vue'

const apiUrl = 'http://localhost:49699/api'

const serviceUrl = `${apiUrl}/Widgets/`

export default {
  name: 'WidgetService',
  get (id) {
    return Vue.http.get(`${serviceUrl}${id}`)
  },
  getList () {
    return Vue.http.get(`${serviceUrl}`)
  },
  post (item) {
    return Vue.http.post(`${serviceUrl}`, item)
  },
  put (item) {
    return Vue.http.put(`${serviceUrl}${item.id}`, item)
  },
  delete (id) {
    return Vue.http.delete(`${serviceUrl}${id}`)
  }
}
