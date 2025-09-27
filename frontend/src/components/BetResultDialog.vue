<script lang="ts" setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps<{
  modelValue: boolean;
  betType: "color" | "parity" | "number" | null;
  betColor: string | null;
  betParity: string | null;
  betNumber: number | null;
  resultMessage: string;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "exit"): void;
}>();

const closeDialog = () => {
  emit("update:modelValue", false);
};
</script>

<template>
  <v-dialog persistent :model-value="modelValue" max-width="400">
    <v-card>
      <div class="d-flex align-center justify-space-between">
        <v-card-title class="text-h6">Resultado de la Apuesta</v-card-title>
        <v-btn
          class="mr-3"
          size="30"
          icon="mdi-close"
          variant="tonal"
          @click="$emit('update:modelValue', false)"
        />
      </div>
      <v-card-text>
        <div v-if="betType === 'color'">
          Apuesta realizada: Color
          <br />
          Selección: <strong>{{ betColor }}</strong>
        </div>
        <div v-else-if="betType === 'parity'">
          Apuesta realizada: Paridad
          <br />
          Selección: <strong>{{ betParity }}</strong>
        </div>
        <div v-else-if="betType === 'number'">
          Apuesta realizada: Número
          <br />
          Selección: <strong>{{ betNumber }}</strong>
        </div>

        <p class="mt-4">{{ resultMessage }}</p>
      </v-card-text>
      <v-card-actions>
        <v-btn variant="elevated" color="error" @click="emit('exit')"
          >Salir</v-btn
        >
        <v-spacer />
        <v-btn variant="elevated" color="primary" @click="closeDialog"
          >Continuar</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
