<template lang="html" src="./Home.html"></template>
<style scoped src="./Home.css"></style>

<script>
import WidgetService from '../services/widget-service';

export default {
  name: 'Home',
  data() {
    return {
      list: null,
      error: null,
      stuff: true
    };
  },
  created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      console.log('fetchData');
      WidgetService.getList(this.list).then(
        response => {
          this.list = response.data;
        },
        error => {
          this.error = error;
        }
      );
    }
  }
};
</script>
