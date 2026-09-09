<script setup>
import { ref, onMounted } from 'vue'
import { getWorkOrder, changeWorkOrderStatus, getWorkOrderActivities } from '@/api/workOrderApi'
import { Haptics, ImpactStyle } from '@capacitor/haptics'

const props = defineProps({ id: { type: Number, required: true } })
const emit = defineEmits(['back'])

const statusOptions = ['Pending', 'InProgress', 'Done', 'Canceled']
const workOrder = ref(null)
const activities = ref([])
const newStatus = ref('')
const note = ref('')
const loading = ref(false)
const updating = ref(false)
const error = ref('')

async function load() {
  loading.value = true
  error.value = ''
  try {
    workOrder.value = await getWorkOrder(props.id)
    newStatus.value = workOrder.value.status
    activities.value = await getWorkOrderActivities(props.id)
  } catch {
    error.value = 'Failed to load work order.'
  } finally {
    loading.value = false
  }
}

async function updateStatus() {
  updating.value = true
  error.value = ''
  try {
    workOrder.value = await changeWorkOrderStatus(props.id, { status: newStatus.value, note: note.value || null })
    note.value = ''
    activities.value = await getWorkOrderActivities(props.id)
    await Haptics.impact({ style: ImpactStyle.Medium })
  } catch {
    error.value = 'Failed to update status.'
  } finally {
    updating.value = false
  }
}

onMounted(load)
</script>

<template>
  <div>
    <button @click="emit('back')">&larr; Back to list</button>

    <p v-if="loading" class="muted">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>

    <template v-else-if="workOrder">
      <div class="detail-header">
        <h2>{{ workOrder.title }}</h2>
        <span class="badge" :class="workOrder.status">{{ workOrder.status }}</span>
      </div>
      <p class="description">{{ workOrder.description || 'No description provided.' }}</p>

      <dl class="meta">
        <div><dt>Assigned To</dt><dd>{{ workOrder.assignedTo || '—' }}</dd></div>
        <div><dt>Scheduled</dt><dd>{{ workOrder.scheduledDate ? new Date(workOrder.scheduledDate).toLocaleDateString() : '—' }}</dd></div>
        <div><dt>Created</dt><dd>{{ workOrder.createdAt ? new Date(workOrder.createdAt).toLocaleString() : '—' }}</dd></div>
      </dl>

      <section class="status-update">
        <h3>Update Status</h3>
        <div class="status-controls">
          <select v-model="newStatus">
            <option v-for="s in statusOptions" :key="s" :value="s">{{ s }}</option>
          </select>
          <input v-model="note" placeholder="Optional note" />
          <button class="primary" @click="updateStatus" :disabled="updating || newStatus === workOrder.status">
            {{ updating ? 'Updating…' : 'Update' }}
          </button>
        </div>
      </section>

      <section class="activity">
        <h3>Recent Activity</h3>
        <ul v-if="activities.length" class="timeline">
          <li v-for="a in activities" :key="a.id">
            <div class="timeline-dot"></div>
            <div class="timeline-body">
              <strong>{{ a.type }}</strong>
              <span v-if="a.oldStatus"> {{ a.oldStatus }} &rarr; {{ a.newStatus }}</span>
              <p v-if="a.note" class="note">{{ a.note }}</p>
              <time>{{ new Date(a.createdAt).toLocaleString() }}</time>
            </div>
          </li>
        </ul>
        <p v-else class="muted">No activity yet.</p>
      </section>
    </template>
  </div>
</template>

<style scoped>
.detail-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-top: 1rem;
}
.detail-header h2 { margin: 0; }
.description { color: var(--color-muted); }

.meta {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
  margin: 1.5rem 0;
  padding: 1rem;
  background: var(--color-bg);
  border-radius: var(--radius);
}
.meta dt { font-size: 0.75rem; text-transform: uppercase; color: var(--color-muted); }
.meta dd { margin: 0.15rem 0 0; font-weight: 600; }

.status-update { margin-bottom: 1.5rem; }
.status-controls { display: flex; gap: 0.5rem; }

.timeline { list-style: none; padding: 0; margin: 0; }
.timeline li { display: flex; gap: 0.75rem; padding: 0.75rem 0; border-bottom: 1px solid var(--color-border); }
.timeline-dot {
  width: 10px; height: 10px; border-radius: 50%;
  background: var(--color-primary); margin-top: 0.4rem; flex-shrink: 0;
}
.timeline-body time { display: block; font-size: 0.75rem; color: var(--color-muted); }
.timeline-body .note { margin: 0.2rem 0; }

.badge {
  display: inline-block;
  padding: 0.2rem 0.6rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}
.badge.Pending { background: #fef3c7; color: #92400e; }
.badge.InProgress { background: #dbeafe; color: #1e40af; }
.badge.Done { background: #dcfce7; color: #166534; }
.badge.Canceled { background: #fee2e2; color: #991b1b; }

.muted { color: var(--color-muted); }
.error { color: var(--color-danger); background: var(--color-danger-bg); padding: 0.5rem 0.75rem; border-radius: var(--radius); }
</style>