<template lang="html" src="./Home.html"></template>
<style scoped src="./Home.css"></style>

<script src="~/signalr.js"></script>

<script>
import WidgetService from '../services/widget-service';
const signalR = require('@aspnet/signalr');
import Vue from 'vue';

export default {
  name: 'Home',
  data() {
    return {
      connection: null,
      list: [],
      error: null,
      stuff: true,
    };
  },
  created() {
    this.fetchData();

    // Creates and starts a connection.
    this.connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:49699/widgethub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    var thisVue = this;

    this.connection.on("Create", function (item) {
      console.log('Create recieved: ', item);
      thisVue.list.unshift(item);
    });

    this.connection.on("Update", function (item) {
      console.log('Update received: ', item);

      var index = thisVue.list.findIndex(x => x.id === item.id);
      console.log('index: ', index);
      if (index !== -1) {
        var local = thisVue.list[index];

        local.processing = item.processing;
        // TODO: Why is this not working --> Vue.set(thisVue.list, index, item);
      } else {
        console.log('Updating non-existing item....');
        thisVue.list.unshift(item);
      }
    });

    this.connection.start().catch(function(err) {
      return console.error('error', err);
    });
  },
  mounted: function() {
    return;
    // ---------
    // Call client methods from hub
    // ---------
  },
  methods: {
    fetchData() {
      console.log('fetchData');
      return; // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
      WidgetService.getList(this.list).then(
        response => {
          this.list = response.data;
        },
        error => {
          this.error = error;
        }
      );
    },
    addClicked: function () {
      var item = {
        "id": 0,
        "name": "Foggles",
        "processing": true,
        "dateCreated": "2019-02-09T01:22:20.512Z"
      };
      WidgetService.post(item).then(
        response => {
          console.log('response', response);
        }, error => {
          console.log('error: ', error);
          //this.error = 'An exception has occurred #' + error.status + ': ' + error.statusText
        });
    },
  }
};
</script>
