<template>
  <div>
    <MessageItem 
      v-for="(msg, index) in answers" 
      :key="index" 
      :msg="msg"
    />
    <LoadingIndicator v-if="loading" />
  </div>
</template>

<script setup lang="ts">
import MessageItem from './MessageItem.vue';
import LoadingIndicator from './LoadingIndicator.vue';
import type { AssistantResponse } from '@/types/AssistantResponse';


interface Props {
  answers: AssistantResponse[];
  loading: boolean;
}

const props = defineProps<Props>();
</script>

<style scoped>
/* optional styles */
</style>
