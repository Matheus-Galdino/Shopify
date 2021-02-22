<template>
  <div class="wrapper" :style="`--borderColor: ${borderColor}`">
    <span
      class="material-icons"
      :style="`--iconColor: ${iconColor}`"
      v-show="isChecked"
      >done</span
    >
  </div>
</template>

<script>
export default {
  name: "Checkbox",
  props: {
    isChecked: {
      type: Boolean,
      default: false,
    },
    borderColor: {
      type: String,
    },
    iconColor: {
      type: String,
    },
  },
};
</script>

<style scoped>
.wrapper {
  display: grid;
  place-content: center;

  width: 2.5rem;
  height: 2.5rem;
  max-width: 2.5rem;
  max-height: 2.5rem;

  padding: 0.2rem;
  cursor: pointer;

  border-radius: 6px;
  border: 2px solid var(--borderColor);
}

.wrapper span {
  color: var(--iconColor);
  font-size: 2rem;
}
</style>
