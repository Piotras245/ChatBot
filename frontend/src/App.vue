<template>  
  <ChatTest></ChatTest>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import ChatTest from './components/dxChatTest.vue'

const userChatTypingUsers = ref([]);
const supportChatTypingUsers = ref([]);

function userChatTypingEnd() {
  supportChatTypingUsers.value = [];
}

function supportChatTypingEnd() {
  userChatTypingUsers.value = [];
}
</script>
