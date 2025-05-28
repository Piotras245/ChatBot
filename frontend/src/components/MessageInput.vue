<template>
  <div class="w-full max-w-2xl mb-4 border-gray-200 rounded-xl ring-0 border-0 bg-[#A4DFCD] dark:bg-gray-700">
    <div class="py-2 bg-[#A4DFCD] rounded-xl dark:bg-gray-800 ring-0 border-0">
      <div class="grid grid-cols-8 gap-1 px-2 bg-[#A4DFCD] ring-0 border-0">
        <textarea 
          :modelValue="input" 
          @keydown.enter.prevent="onSend" 
          class="col-span-7 bg-[#A4DFCD] w-full max-h-[200px] px-2 py-2 text-sm text-[#133730] dark:bg-gray-800 focus:outline-none border-0 dark:text-white resize-none"
          placeholder="Ask a question..." 
          required
        />
        <button @click="onSend" class="group inline-flex items-center py-2.5 px-4 text-xs font-medium text-white rounded-lg ml-2 bg-[#A4DFCD] ring-0 border-0">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="#5F887C" class="h-10 w-8 group-hover:fill-sky-100 cursor-pointer transition-colors">
            <path d="M3 21L21 12L3 3V10L13.5 12L3 14V21Z" />
          </svg>
        </button>
      </div>
    </div>
  </div>

</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

const props = defineProps({
  input: String
});

const emit = defineEmits(['send']);

const onSend = () => {
  emit('send');
};
</script>

