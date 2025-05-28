<template>
  <div class="flex flex-col w-full max-w-[500px] leading-1.5 p-4 mt-4 border-gray-200 bg-[#3D7068] rounded-xl dark:bg-gray-700">
    <div class="flex items-center space-x-2 rtl:space-x-reverse">
      <span class="text-sm font-semibold text-sky-100 dark:text-white">Chat response</span>
      <span class="text-sm font-normal text-sky-100 dark:text-gray-400">{{ msg.timestamp }}</span>
    </div>
    <p class="mt-2 text-sm font-normal py-2.5 text-sky-50 dark:text-white">{{ msg.text }}</p>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';

const props = defineProps({
  msg: {
    type: Object,
    required: true
  }
});
</script>

<style scoped>
/* Styles specific to each message */
</style>
