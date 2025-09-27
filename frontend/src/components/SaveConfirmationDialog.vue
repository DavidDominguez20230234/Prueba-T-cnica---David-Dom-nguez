<script lang="ts" setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps<{
  modelValue: boolean;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "save"): void;
  (e: "discard"): void;
}>();
</script>

<template>
  <v-dialog persistent :model-value="modelValue" max-width="400">
    <v-card>
      <div class="d-flex align-center justify-space-between">
        <v-card-title class="text-h6">¿Deseas guardar la partida?</v-card-title>
        <v-btn
          class="mr-3"
          size="30"
          icon="mdi-close"
          variant="tonal"
          @click="$emit('update:modelValue', false)"
        />
      </div>
      <v-card-text> Si no guardas, el progreso se perderá. </v-card-text>
      <v-card-actions>
        <v-btn color="error" variant="elevated" @click="emit('discard')">
          No guardar
        </v-btn>
        <v-spacer />
        <v-btn color="primary" variant="elevated" @click="emit('save')">
          Guardar y salir
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
